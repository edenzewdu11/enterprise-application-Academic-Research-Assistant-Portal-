using ARAP.Modules.AcademicIntegrity.Domain.Aggregates;
using ARAP.Modules.AcademicIntegrity.Domain.ValueObjects;

namespace ARAP.Modules.AcademicIntegrity.Domain.Repositories;

/// <summary>
/// Repository interface for plagiarism checks
/// </summary>
public interface IPlagiarismCheckRepository
{
    Task<PlagiarismCheck?> GetByIdAsync(PlagiarismCheckId id, CancellationToken cancellationToken = default);
    Task<PlagiarismCheck?> GetByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PlagiarismCheck>> GetByProposalIdAsync(Guid proposalId, CancellationToken cancellationToken = default);
    Task AddAsync(PlagiarismCheck check, CancellationToken cancellationToken = default);
    Task UpdateAsync(PlagiarismCheck check, CancellationToken cancellationToken = default);
}
