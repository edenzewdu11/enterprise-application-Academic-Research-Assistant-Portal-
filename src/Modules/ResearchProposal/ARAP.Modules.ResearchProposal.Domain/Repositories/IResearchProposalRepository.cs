using ARAP.Modules.ResearchProposal.Domain.Aggregates;
using ARAP.Modules.ResearchProposal.Domain.ValueObjects;

namespace ARAP.Modules.ResearchProposal.Domain.Repositories;

/// <summary>
/// Repository interface for ResearchProposal aggregate
/// Follows Repository pattern - only aggregate roots have repositories
/// </summary>
public interface IResearchProposalRepository
{
    Task<Aggregates.ResearchProposal?> GetByIdAsync(ResearchProposalId id, CancellationToken cancellationToken = default);
    
    Task<Aggregates.ResearchProposal?> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<Aggregates.ResearchProposal>> GetByAdvisorIdAsync(Guid advisorId, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<Aggregates.ResearchProposal>> GetByStateAsync(ProposalState state, CancellationToken cancellationToken = default);
    
    Task AddAsync(Aggregates.ResearchProposal proposal, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(Aggregates.ResearchProposal proposal, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(Aggregates.ResearchProposal proposal, CancellationToken cancellationToken = default);
}
