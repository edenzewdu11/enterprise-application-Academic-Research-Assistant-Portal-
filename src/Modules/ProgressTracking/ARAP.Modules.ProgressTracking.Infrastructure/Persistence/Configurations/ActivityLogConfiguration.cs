using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ARAP.SharedKernel;
using ARAP.Modules.ProgressTracking.Domain.Aggregates;
using ARAP.Modules.ProgressTracking.Domain.ValueObjects;

namespace ARAP.Modules.ProgressTracking.Infrastructure.Persistence.Configurations;

internal sealed class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.ToTable("activity_logs");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => ActivityLogId.Create(value))
            .HasColumnName("id");

        builder.Property(a => a.ProposalId)
            .HasColumnName("proposal_id")
            .IsRequired();

        builder.Property(a => a.StudentId)
            .HasColumnName("student_id")
            .IsRequired();

        builder.Property(a => a.Type)
            .HasConversion(
                type => type.Name,
                name => Enumeration.FromName<ActivityType>(name)!)
            .HasColumnName("activity_type")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.Description)
            .HasConversion(
                desc => desc.Value,
                value => ActivityDescription.Create(value).Value!)
            .HasColumnName("description")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(a => a.Progress)
            .HasConversion(
                progress => progress.Value,
                value => ProgressPercentage.Create(value).Value!)
            .HasColumnName("progress_percentage")
            .IsRequired();

        builder.Property(a => a.HoursSpent)
            .HasColumnName("hours_spent")
            .IsRequired();

        builder.Property(a => a.LoggedAt)
            .HasColumnName("logged_at")
            .IsRequired();

        builder.Property(a => a.CompletedAt)
            .HasColumnName("completed_at");

        builder.Property(a => a.Version)
            .HasColumnName("version")
            .IsConcurrencyToken();

        builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(a => a.LastModifiedAt)
            .HasColumnName("last_modified_at");

        builder.Ignore(a => a.DomainEvents);

        builder.HasIndex(a => a.ProposalId)
            .HasDatabaseName("ix_activity_logs_proposal_id");

        builder.HasIndex(a => a.StudentId)
            .HasDatabaseName("ix_activity_logs_student_id");

        builder.HasIndex(a => a.LoggedAt)
            .HasDatabaseName("ix_activity_logs_logged_at");
    }
}
