using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace ARAP.Infrastructure.Outbox;

/// <summary>
/// Extension methods for configuring Transactional Outbox infrastructure
/// </summary>
public static class OutboxExtensions
{
    /// <summary>
    /// Registers Transactional Outbox infrastructure with Quartz scheduler
    /// </summary>
    public static IServiceCollection AddOutbox(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register Outbox DbContext
        var connectionString = configuration.GetConnectionString("OutboxDb")
            ?? configuration.GetConnectionString("ResearchProposalDb"); // Fallback to main DB

        services.AddDbContext<OutboxDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Configure Quartz for background job processing
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule
                                .WithIntervalInSeconds(10) // Process every 10 seconds
                                .RepeatForever()));
        });

        // Add Quartz hosted service
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }

    /// <summary>
    /// Intercepts SaveChanges to capture domain events and store them in outbox
    /// Call this method in each module's DbContext.SaveChangesAsync
    /// </summary>
    public static async Task<int> SaveChangesWithOutboxAsync(
        this DbContext context,
        OutboxDbContext outboxContext,
        CancellationToken cancellationToken = default)
    {
        // Get domain events from all tracked entities that have domain events
        var domainEvents = new List<object>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is ARAP.SharedKernel.IHasDomainEvents entityWithEvents)
            {
                var events = entityWithEvents.GetDomainEvents();
                domainEvents.AddRange(events);
                entityWithEvents.ClearDomainEvents();
            }
        }

        // Convert domain events to outbox messages
        var outboxMessages = domainEvents.Select(domainEvent => new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = domainEvent.GetType().FullName!,
            Content = System.Text.Json.JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
            OccurredOnUtc = DateTime.UtcNow,
            RetryCount = 0
        }).ToList();

        // Add outbox messages to outbox context
        await outboxContext.OutboxMessages.AddRangeAsync(outboxMessages, cancellationToken);

        // Save both contexts in the same transaction (if same database)
        var result = await context.SaveChangesAsync(cancellationToken);
        await outboxContext.SaveChangesAsync(cancellationToken);

        return result;
    }
}
