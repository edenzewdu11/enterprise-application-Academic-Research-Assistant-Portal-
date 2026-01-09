using ARAP.Modules.DocumentReview.Domain.Aggregates;
using ARAP.Modules.DocumentReview.Domain.ValueObjects;

namespace ARAP.Modules.DocumentReview.Domain.Repositories;

/// <summary>
/// Repository interface for Document aggregate
/// </summary>
public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(DocumentId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Document>> GetByProposalIdAsync(Guid proposalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Document>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Document>> GetByReviewerIdAsync(Guid reviewerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Document>> GetByStatusAsync(ReviewStatus status, CancellationToken cancellationToken = default);
    Task AddAsync(Document document, CancellationToken cancellationToken = default);
    Task UpdateAsync(Document document, CancellationToken cancellationToken = default);
    Task DeleteAsync(Document document, CancellationToken cancellationToken = default);
}
