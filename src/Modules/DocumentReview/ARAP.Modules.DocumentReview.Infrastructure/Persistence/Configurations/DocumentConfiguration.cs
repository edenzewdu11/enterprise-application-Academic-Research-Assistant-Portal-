using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ARAP.Modules.DocumentReview.Domain.Aggregates;
using ARAP.Modules.DocumentReview.Domain.ValueObjects;

namespace ARAP.Modules.DocumentReview.Infrastructure.Persistence.Configurations;

public sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("documents");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => DocumentId.Create(value))
            .HasColumnName("id");

        builder.Property(d => d.ProposalId)
            .IsRequired()
            .HasColumnName("proposal_id");

        builder.Property(d => d.StudentId)
            .IsRequired()
            .HasColumnName("student_id");

        builder.Property(d => d.Type)
            .HasConversion(
                type => type.Value,
                value => DocumentType.FromString(value))
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("type");

        builder.Property(d => d.FileName)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("file_name");

        builder.Property(d => d.FileUrl)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("file_url");

        builder.Property(d => d.Status)
            .HasConversion(
                status => status.Value,
                value => ReviewStatus.FromString(value))
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("status");
            
        builder.Property(d => d.Version)
            .IsRequired()
            .HasColumnName("version");

        builder.Property(d => d.SubmittedAt)
            .IsRequired()
            .HasColumnName("submitted_at");

        builder.Property(d => d.ReviewedAt)
            .HasColumnName("reviewed_at");

        builder.Property(d => d.ReviewerId)
            .HasColumnName("reviewer_id");

        builder.Property(d => d.Feedback)
            .HasConversion(
                feedback => feedback != null ? feedback.Value : null,
                value => value != null ? FeedbackComment.Create(value).Value! : null)
            .HasMaxLength(2000)
            .HasColumnName("feedback");

        builder.Property(d => d.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(d => d.LastModifiedAt)
            .HasColumnName("last_modified_at");

        builder.Ignore(d => d.DomainEvents);

        builder.HasIndex(d => d.ProposalId);
        builder.HasIndex(d => d.StudentId);
        builder.HasIndex(d => d.ReviewerId);
        builder.HasIndex(d => d.Status);
    }
}
