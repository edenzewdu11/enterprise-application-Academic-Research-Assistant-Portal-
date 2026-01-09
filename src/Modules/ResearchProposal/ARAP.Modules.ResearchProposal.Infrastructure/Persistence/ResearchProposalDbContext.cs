using Microsoft.EntityFrameworkCore;
using ARAP.Modules.ResearchProposal.Infrastructure.Persistence.Configurations;
using ResearchProposalAggregate = ARAP.Modules.ResearchProposal.Domain.Aggregates.ResearchProposal;

namespace ARAP.Modules.ResearchProposal.Infrastructure.Persistence;

/// <summary>
/// DbContext for ResearchProposal bounded context
/// Manages persistence for ResearchProposal aggregate and related entities
/// </summary>
public sealed class ResearchProposalDbContext : DbContext
{
    public DbSet<ResearchProposalAggregate> ResearchProposals => Set<ResearchProposalAggregate>();

    public ResearchProposalDbContext(DbContextOptions<ResearchProposalDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set default schema for this bounded context
        modelBuilder.HasDefaultSchema("research_proposal");

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResearchProposalDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
