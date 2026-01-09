using ARAP.Modules.Notifications.Domain.Aggregates;
using ARAP.Modules.Notifications.Domain.ValueObjects;

namespace ARAP.Modules.Notifications.Domain.Repositories;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(NotificationId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Notification>> GetByRecipientAsync(Guid recipientId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Notification>> GetUnreadByRecipientAsync(Guid recipientId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(Guid recipientId, CancellationToken cancellationToken = default);
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default);
}
