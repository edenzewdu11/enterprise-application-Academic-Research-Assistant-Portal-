using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ARAP.Modules.Notifications.Domain.Aggregates;
using ARAP.Modules.Notifications.Domain.ValueObjects;
using ARAP.SharedKernel;

namespace ARAP.Modules.Notifications.Infrastructure.Persistence.Configurations;

internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasConversion(
                id => id.Value,
                value => NotificationId.From(value))
            .HasColumnName("id");

        builder.Property(n => n.RecipientId)
            .IsRequired()
            .HasColumnName("recipient_id");

        builder.Property(n => n.Type)
            .IsRequired()
            .HasConversion(
                type => type.Value,
                value => Enumeration.FromValue<NotificationType>(value)!)
            .HasColumnName("type");

        builder.OwnsOne(n => n.Content, content =>
        {
            content.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("title");

            content.Property(c => c.Message)
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnName("message");

            content.Property(c => c.ActionUrl)
                .HasMaxLength(500)
                .HasColumnName("action_url");
        });

        builder.Property(n => n.Channel)
            .IsRequired()
            .HasConversion(
                channel => channel.Value,
                value => Enumeration.FromValue<DeliveryChannel>(value)!)
            .HasColumnName("channel");

        builder.Property(n => n.RelatedEntityId)
            .HasColumnName("related_entity_id");

        builder.Property(n => n.IsRead)
            .IsRequired()
            .HasColumnName("is_read");

        builder.Property(n => n.ReadAt)
            .HasColumnName("read_at");

        builder.Property(n => n.SentAt)
            .IsRequired()
            .HasColumnName("sent_at");

        builder.Property(n => n.ExpiresAt)
            .HasColumnName("expires_at");

        builder.Ignore(n => n.DomainEvents);

        builder.HasIndex(n => n.RecipientId);
        builder.HasIndex(n => new { n.RecipientId, n.IsRead });
        builder.HasIndex(n => n.Type);
        builder.HasIndex(n => n.RelatedEntityId);
    }
}
