using ARAP.SharedKernel;
using ARAP.Modules.Notifications.Domain.ValueObjects;

namespace ARAP.Modules.Notifications.Domain.Events;

public sealed record NotificationReadEvent(
    NotificationId NotificationId,
    Guid RecipientId,
    DateTime ReadAt
) : DomainEvent;
