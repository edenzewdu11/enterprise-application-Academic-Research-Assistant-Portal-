using Microsoft.EntityFrameworkCore;
using ARAP.Modules.Notifications.Domain.Aggregates;
using ARAP.Modules.Notifications.Domain.Repositories;
using ARAP.Modules.Notifications.Domain.ValueObjects;
using ARAP.Modules.Notifications.Infrastructure.Persistence;

namespace ARAP.Modules.Notifications.Infrastructure.Persistence.Repositories;

internal sealed class NotificationRepository : INotificationRepository
{
    private readonly NotificationsDbContext _context;

    public NotificationRepository(NotificationsDbContext context)
    {
        _context = context;
    }

    public async Task<Notification?> GetByIdAsync(NotificationId id, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Notification>> GetByRecipientAsync(Guid recipientId, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .Where(n => n.RecipientId == recipientId)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Notification>> GetUnreadByRecipientAsync(Guid recipientId, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .Where(n => n.RecipientId == recipientId && !n.IsRead)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUnreadCountAsync(Guid recipientId, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .CountAsync(n => n.RecipientId == recipientId && !n.IsRead, cancellationToken);
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
