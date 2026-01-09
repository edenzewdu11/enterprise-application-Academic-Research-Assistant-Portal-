using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Quartz;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ARAP.Infrastructure.Outbox;

/// <summary>
/// Background job that publishes outbox messages to the event bus
/// Runs periodically using Quartz scheduler
/// </summary>
[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob : IJob
{
    private readonly OutboxDbContext _dbContext;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;
    private readonly IConfiguration _configuration;
    private readonly ResiliencePipeline _resiliencePipeline;
    private IConnection? _connection;
    private IChannel? _channel;

    private const int BatchSize = 20;
    private const int MaxRetries = 3;

    public ProcessOutboxMessagesJob(
        OutboxDbContext dbContext,
        ILogger<ProcessOutboxMessagesJob> logger,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _logger = logger;
        _configuration = configuration;

        // Configure retry policy with exponential backoff
        _resiliencePipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                OnRetry = args =>
                {
                    _logger.LogWarning(
                        "Retry attempt {AttemptNumber} after {RetryDelay}ms due to: {Exception}",
                        args.AttemptNumber,
                        args.RetryDelay.TotalMilliseconds,
                        args.Outcome.Exception?.Message);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Processing outbox messages...");

        // Initialize RabbitMQ connection if not already connected
        await EnsureRabbitMQConnectionAsync();

        // Fetch unprocessed messages
        var messages = await _dbContext.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null && m.RetryCount < MaxRetries)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(BatchSize)
            .ToListAsync(context.CancellationToken);

        if (!messages.Any())
        {
            _logger.LogDebug("No outbox messages to process");
            return;
        }

        _logger.LogInformation("Found {Count} outbox messages to process", messages.Count);

        foreach (var message in messages)
        {
            try
            {
                await _resiliencePipeline.ExecuteAsync(async ct =>
                {
                    await PublishMessageAsync(message, ct);
                }, context.CancellationToken);

                message.ProcessedOnUtc = DateTime.UtcNow;
                _logger.LogInformation(
                    "Successfully processed outbox message {MessageId} of type {Type}",
                    message.Id,
                    message.Type);
            }
            catch (Exception ex)
            {
                message.RetryCount++;
                message.Error = ex.Message;

                _logger.LogError(
                    ex,
                    "Failed to process outbox message {MessageId} after {RetryCount} retries",
                    message.Id,
                    message.RetryCount);

                if (message.RetryCount >= MaxRetries)
                {
                    _logger.LogError(
                        "Outbox message {MessageId} exceeded max retries and will be skipped",
                        message.Id);
                }
            }
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
        _logger.LogInformation("Outbox processing completed");
    }

    private async Task EnsureRabbitMQConnectionAsync()
    {
        if (_channel != null && _channel.IsOpen)
            return;

        try
        {
            var hostName = _configuration["RabbitMQ:HostName"] ?? "localhost";
            var port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672");
            var userName = _configuration["RabbitMQ:UserName"] ?? "admin";
            var password = _configuration["RabbitMQ:Password"] ?? "admin";
            var virtualHost = _configuration["RabbitMQ:VirtualHost"] ?? "/";

            _logger.LogInformation("Attempting RabbitMQ connection: Host={Host}, Port={Port}, User={User}, VHost={VHost}", 
                hostName, port, userName, virtualHost);

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            var exchange = _configuration["RabbitMQ:Exchange"] ?? "arap.events";
            var exchangeType = _configuration["RabbitMQ:ExchangeType"] ?? "topic";

            // Declare the exchange
            await _channel.ExchangeDeclareAsync(
                exchange: exchange,
                type: exchangeType,
                durable: true,
                autoDelete: false);

            _logger.LogInformation("RabbitMQ connection established to {HostName}", factory.HostName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to establish RabbitMQ connection");
            throw;
        }
    }

    private async Task PublishMessageAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        if (_channel == null || !_channel.IsOpen)
        {
            throw new InvalidOperationException("RabbitMQ channel is not available");
        }

        var exchange = _configuration["RabbitMQ:Exchange"] ?? "arap.events";
        
        // Extract routing key from event type (e.g., "ProposalCreatedEvent" -> "proposal.created")
        var routingKey = GetRoutingKeyFromEventType(message.Type);

        var body = Encoding.UTF8.GetBytes(message.Content);

        var properties = new BasicProperties
        {
            Persistent = true,
            MessageId = message.Id.ToString(),
            Timestamp = new AmqpTimestamp(((DateTimeOffset)message.OccurredOnUtc).ToUnixTimeSeconds()),
            ContentType = "application/json",
            Type = message.Type
        };

        await _channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);

        _logger.LogInformation(
            "Published message {MessageId} of type {Type} to exchange {Exchange} with routing key {RoutingKey}",
            message.Id,
            message.Type,
            exchange,
            routingKey);
    }

    private static string GetRoutingKeyFromEventType(string eventType)
    {
        // Convert "ProposalCreatedEvent" to "proposal.created"
        // Remove "Event" suffix if present
        var withoutSuffix = eventType.EndsWith("Event") 
            ? eventType.Substring(0, eventType.Length - 5) 
            : eventType;

        // Convert PascalCase to dot.separated.lowercase
        var result = new StringBuilder();
        for (int i = 0; i < withoutSuffix.Length; i++)
        {
            if (i > 0 && char.IsUpper(withoutSuffix[i]))
            {
                result.Append('.');
            }
            result.Append(char.ToLower(withoutSuffix[i]));
        }

        return result.ToString();
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }

        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}
