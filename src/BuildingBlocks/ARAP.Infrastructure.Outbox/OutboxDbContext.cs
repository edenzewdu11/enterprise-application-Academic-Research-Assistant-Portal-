using Microsoft.EntityFrameworkCore;

namespace ARAP.Infrastructure.Outbox;

/// <summary>
/// DbContext for Outbox pattern - stores unpublished domain events
/// </summary>
public sealed class OutboxDbContext : DbContext
{
    /// <summary>
    /// Initializes new instance of OutboxDbContext
    /// </summary>
    public OutboxDbContext(DbContextOptions<OutboxDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the outbox messages DbSet
    /// </summary>
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    /// <summary>
    /// Configures the model for the outbox context
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("outbox");

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("outbox_messages");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.Content)
                .IsRequired();

            entity.Property(e => e.OccurredOnUtc)
                .IsRequired();

            entity.Property(e => e.ProcessedOnUtc);

            entity.Property(e => e.Error)
                .HasMaxLength(2000);

            entity.Property(e => e.RetryCount)
                .IsRequired()
                .HasDefaultValue(0);

            // Indexes for efficient querying
            entity.HasIndex(e => e.ProcessedOnUtc)
                .HasDatabaseName("ix_outbox_messages_processed_on_utc");

            entity.HasIndex(e => e.OccurredOnUtc)
                .HasDatabaseName("ix_outbox_messages_occurred_on_utc");
        });
    }
}
