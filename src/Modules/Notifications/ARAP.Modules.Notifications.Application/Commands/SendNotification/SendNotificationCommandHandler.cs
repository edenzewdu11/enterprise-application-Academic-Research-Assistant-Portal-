using ARAP.SharedKernel;
using ARAP.Modules.Notifications.Domain.Aggregates;
using ARAP.Modules.Notifications.Domain.Repositories;
using ARAP.Modules.Notifications.Domain.ValueObjects;
using MediatR;

namespace ARAP.Modules.Notifications.Application.Commands.SendNotification;

internal sealed class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Result<Guid>>
{
    private readonly INotificationRepository _notificationRepository;

    public SendNotificationCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<Guid>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notificationTypeResult = NotificationType.FromName<NotificationType>(request.Type);
        if (notificationTypeResult == null)
            return Result.Failure<Guid>($"Invalid notification type: {request.Type}");

        var contentResult = NotificationContent.Create(request.Title, request.Message, request.ActionUrl);
        if (contentResult.IsFailure)
            return Result.Failure<Guid>(contentResult.Error!);

        var channelResult = DeliveryChannel.FromName<DeliveryChannel>(request.Channel);
        if (channelResult == null)
            return Result.Failure<Guid>($"Invalid delivery channel: {request.Channel}");

        var notificationResult = Notification.Create(
            request.RecipientId,
            notificationTypeResult,
            contentResult.Value!,
            channelResult,
            request.RelatedEntityId,
            request.ExpiresAt);

        if (notificationResult.IsFailure)
            return Result.Failure<Guid>(notificationResult.Error!);

        await _notificationRepository.AddAsync(notificationResult.Value!, cancellationToken);

        return Result.Success(notificationResult.Value!.Id.Value);
    }
}
