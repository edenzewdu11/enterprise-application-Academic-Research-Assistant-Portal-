using Microsoft.EntityFrameworkCore;
using ARAP.Modules.Notifications.Domain.Aggregates;

namespace ARAP.Modules.Notifications.Infrastructure.Persistence;

internal sealed class NotificationsDbContext : DbContext
{
    public DbSet<Notification> Notifications { get; set; } = null!;

    public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("notifications");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationsDbContext).Assembly);
    }
}
