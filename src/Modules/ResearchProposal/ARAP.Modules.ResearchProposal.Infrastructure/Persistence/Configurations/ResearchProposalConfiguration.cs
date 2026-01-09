using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ARAP.Modules.ResearchProposal.Domain.ValueObjects;
using ARAP.Modules.ResearchProposal.Domain.Entities;
using ResearchProposalAggregate = ARAP.Modules.ResearchProposal.Domain.Aggregates.ResearchProposal;

namespace ARAP.Modules.ResearchProposal.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for ResearchProposal aggregate root
/// Maps domain model to database schema with value object conversions
/// </summary>
public sealed class ResearchProposalConfiguration : IEntityTypeConfiguration<ResearchProposalAggregate>
{
    public void Configure(EntityTypeBuilder<ResearchProposalAggregate> builder)
    {
        // Table configuration
        builder.ToTable("research_proposals");

        // Primary key - convert ResearchProposalId value object to Guid
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ResearchProposalId.Create(value))
            .HasColumnName("id");

        // Simple properties
        builder.Property(p => p.StudentId)
            .IsRequired()
            .HasColumnName("student_id");

        builder.Property(p => p.AdvisorId)
            .IsRequired()
            .HasColumnName("advisor_id");

        // Value object: ProposalTitle -> string
        builder.Property(p => p.Title)
            .HasConversion(
                title => title.Value,
                value => ProposalTitle.Create(value).Value!)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("title");

        // Value object: Abstract -> string
        builder.Property(p => p.Abstract)
            .HasConversion(
                abs => abs.Value,
                value => Abstract.Create(value).Value!)
            .IsRequired()
            .HasMaxLength(3000)
            .HasColumnName("abstract");

        // Value object: ResearchQuestion -> string
        builder.Property(p => p.ResearchQuestion)
            .HasConversion(
                question => question.Value,
                value => ResearchQuestion.Create(value).Value!)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("research_question");

        // Value object: ProposalState -> string (enum)
        builder.Property(p => p.State)
            .HasConversion(
                state => state.Value,
                value => ProposalState.FromString(value))
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("state");

        // Review comments
        builder.Property(p => p.ReviewComments)
            .HasMaxLength(2000)
            .HasColumnName("review_comments");

        // Timestamps
        builder.Property(p => p.SubmittedAt)
            .HasColumnName("submitted_at");

        builder.Property(p => p.ReviewedAt)
            .HasColumnName("reviewed_at");

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(p => p.LastModifiedAt)
            .HasColumnName("last_modified_at");

        // Optimistic concurrency
        builder.Property(p => p.Version)
            .IsRequired()
            .IsConcurrencyToken()
            .HasColumnName("version");

        // Owned collection: Milestones (owned entities)
        builder.OwnsMany(p => p.Milestones, mb =>
        {
            mb.ToTable("milestones");

            mb.WithOwner()
                .HasForeignKey("research_proposal_id");

            mb.HasKey(m => m.Id);
            
            mb.Property(m => m.Id)
                .HasConversion(
                    id => id.Value,
                    value => MilestoneId.CreateUnique())
                .HasColumnName("id");

            // Value object: MilestoneType -> string
            mb.Property(m => m.Type)
                .HasConversion(
                    type => type.Value,
                    value => MilestoneType.FromString(value))
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("type");

            mb.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("description");

            mb.Property(m => m.Deadline)
                .IsRequired()
                .HasColumnName("deadline");

            mb.Property(m => m.CompletionDate)
                .HasColumnName("completion_date");

            // Value object: MilestoneStatus -> string
            mb.Property(m => m.Status)
                .HasConversion(
                    status => status.Value,
                    value => MilestoneStatus.FromString(value))
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        // Ignore domain events (not persisted)
        builder.Ignore(p => p.DomainEvents);

        // Indexes for common queries
        builder.HasIndex(p => p.StudentId)
            .HasDatabaseName("ix_research_proposals_student_id");

        builder.HasIndex(p => p.AdvisorId)
            .HasDatabaseName("ix_research_proposals_advisor_id");

        builder.HasIndex(p => p.State)
            .HasDatabaseName("ix_research_proposals_state");

        builder.HasIndex(p => p.CreatedAt)
            .HasDatabaseName("ix_research_proposals_created_at");
    }
}
