using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ARAP.SharedKernel;
using ARAP.Modules.AcademicIntegrity.Domain.Aggregates;
using ARAP.Modules.AcademicIntegrity.Domain.ValueObjects;

namespace ARAP.Modules.AcademicIntegrity.Infrastructure.Persistence.Configurations;

internal sealed class PlagiarismCheckConfiguration : IEntityTypeConfiguration<PlagiarismCheck>
{
    public void Configure(EntityTypeBuilder<PlagiarismCheck> builder)
    {
        builder.ToTable("plagiarism_checks");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PlagiarismCheckId.Create(value))
            .HasColumnName("id");

        builder.Property(p => p.DocumentId)
            .HasColumnName("document_id")
            .IsRequired();

        builder.Property(p => p.ProposalId)
            .HasColumnName("proposal_id")
            .IsRequired();

        builder.Property(p => p.InitiatedBy)
            .HasColumnName("initiated_by")
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion(
                status => status.Name,
                name => Enumeration.FromName<CheckStatus>(name)!)
            .HasColumnName("status")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Score)
            .HasConversion(
                score => score != null ? score.Value : (decimal?)null,
                value => value.HasValue ? SimilarityScore.Create(value.Value).Value! : null)
            .HasColumnName("similarity_score")
            .HasPrecision(5, 2);

        builder.Property(p => p.ExternalCheckId)
            .HasColumnName("external_check_id")
            .HasMaxLength(200);

        builder.Property(p => p.Notes)
            .HasConversion(
                notes => notes != null ? notes.Value : null,
                value => value != null ? CheckNotes.Create(value).Value! : null)
            .HasColumnName("notes")
            .HasMaxLength(2000);

        builder.Property(p => p.InitiatedAt)
            .HasColumnName("initiated_at")
            .IsRequired();

        builder.Property(p => p.CompletedAt)
            .HasColumnName("completed_at");

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(p => p.LastModifiedAt)
            .HasColumnName("last_modified_at");

        builder.Property(p => p.Version)
            .HasColumnName("version")
            .IsRequired();

        builder.Ignore(p => p.DomainEvents);

        builder.HasIndex(p => p.DocumentId);
        builder.HasIndex(p => p.ProposalId);
        builder.HasIndex(p => p.Status);
    }
}
