using ARAP.SharedKernel;
using ARAP.Modules.Notifications.Domain.ValueObjects;

namespace ARAP.Modules.Notifications.Domain.Events;

public sealed record NotificationSentEvent(
    NotificationId NotificationId,
    Guid RecipientId,
    NotificationType Type,
    string Title,
    DateTime SentAt
) : DomainEvent;
