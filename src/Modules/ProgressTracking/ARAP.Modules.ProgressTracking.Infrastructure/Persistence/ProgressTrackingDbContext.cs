using Microsoft.EntityFrameworkCore;
using ARAP.Modules.ProgressTracking.Domain.Aggregates;

namespace ARAP.Modules.ProgressTracking.Infrastructure.Persistence;

public sealed class ProgressTrackingDbContext : DbContext
{
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

    public ProgressTrackingDbContext(DbContextOptions<ProgressTrackingDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("progress_tracking");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProgressTrackingDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}
