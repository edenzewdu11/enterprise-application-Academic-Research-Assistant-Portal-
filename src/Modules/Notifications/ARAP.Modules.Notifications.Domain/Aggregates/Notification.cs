using ARAP.SharedKernel;
using ARAP.Modules.Notifications.Domain.Events;
using ARAP.Modules.Notifications.Domain.ValueObjects;

namespace ARAP.Modules.Notifications.Domain.Aggregates;

public sealed class Notification : AggregateRoot<NotificationId>
{
    public Guid RecipientId { get; private set; }
    public NotificationType Type { get; private set; }
    public NotificationContent Content { get; private set; }
    public DeliveryChannel Channel { get; private set; }
    public Guid? RelatedEntityId { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime? ReadAt { get; private set; }
    public DateTime SentAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    // EF Core constructor
    private Notification()
    {
        Content = default!;
        Type = default!;
        Channel = default!;
    }

    private Notification(
        NotificationId id,
        Guid recipientId,
        NotificationType type,
        NotificationContent content,
        DeliveryChannel channel,
        Guid? relatedEntityId,
        DateTime? expiresAt)
        : base(id)
    {
        RecipientId = recipientId;
        Type = type;
        Content = content;
        Channel = channel;
        RelatedEntityId = relatedEntityId;
        IsRead = false;
        SentAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
    }

    public static Result<Notification> Create(
        Guid recipientId,
        NotificationType type,
        NotificationContent content,
        DeliveryChannel channel,
        Guid? relatedEntityId = null,
        DateTime? expiresAt = null)
    {
        if (recipientId == Guid.Empty)
            return Result.Failure<Notification>("Recipient ID is required");

        if (type == null)
            return Result.Failure<Notification>("Notification type is required");

        if (content == null)
            return Result.Failure<Notification>("Notification content is required");

        if (channel == null)
            return Result.Failure<Notification>("Delivery channel is required");

        if (expiresAt.HasValue && expiresAt.Value <= DateTime.UtcNow)
            return Result.Failure<Notification>("Expiration date must be in the future");

        var notification = new Notification(
            NotificationId.New(),
            recipientId,
            type,
            content,
            channel,
            relatedEntityId,
            expiresAt);

        notification.RaiseDomainEvent(new NotificationSentEvent(
            notification.Id,
            recipientId,
            type,
            content.Title,
            notification.SentAt));

        return Result.Success(notification);
    }

    public Result MarkAsRead()
    {
        if (IsRead)
            return Result.Failure("Notification is already marked as read");

        IsRead = true;
        ReadAt = DateTime.UtcNow;

        RaiseDomainEvent(new NotificationReadEvent(Id, RecipientId, ReadAt.Value));

        return Result.Success();
    }

    public bool IsExpired()
    {
        return ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;
    }

    public bool IsActive()
    {
        return !IsExpired();
    }
}
