using Microsoft.EntityFrameworkCore;
using ARAP.Modules.ResearchProposal.Domain.Repositories;
using ARAP.Modules.ResearchProposal.Domain.ValueObjects;
using ResearchProposalAggregate = ARAP.Modules.ResearchProposal.Domain.Aggregates.ResearchProposal;

namespace ARAP.Modules.ResearchProposal.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of ResearchProposal repository
/// Handles data access for ResearchProposal aggregate
/// </summary>
public sealed class ResearchProposalRepository : IResearchProposalRepository
{
    private readonly ResearchProposalDbContext _context;

    public ResearchProposalRepository(ResearchProposalDbContext context)
    {
        _context = context;
    }

    public async Task<ResearchProposalAggregate?> GetByIdAsync(
        ResearchProposalId id, 
        CancellationToken cancellationToken = default)
    {
        return await _context.ResearchProposals
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<ResearchProposalAggregate?> GetByStudentIdAsync(
        Guid studentId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.ResearchProposals
            .FirstOrDefaultAsync(p => p.StudentId == studentId, cancellationToken);
    }

    public async Task<IReadOnlyList<ResearchProposalAggregate>> GetByAdvisorIdAsync(
        Guid advisorId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.ResearchProposals
            .Where(p => p.AdvisorId == advisorId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ResearchProposalAggregate>> GetByStateAsync(
        ProposalState state, 
        CancellationToken cancellationToken = default)
    {
        return await _context.ResearchProposals
            .Where(p => p.State == state)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        ResearchProposalAggregate proposal, 
        CancellationToken cancellationToken = default)
    {
        await _context.ResearchProposals.AddAsync(proposal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        ResearchProposalAggregate proposal, 
        CancellationToken cancellationToken = default)
    {
        _context.ResearchProposals.Update(proposal);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        ResearchProposalAggregate proposal, 
        CancellationToken cancellationToken = default)
    {
        _context.ResearchProposals.Remove(proposal);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
