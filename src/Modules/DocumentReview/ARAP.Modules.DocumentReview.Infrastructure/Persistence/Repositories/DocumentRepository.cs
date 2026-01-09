using Microsoft.EntityFrameworkCore;
using ARAP.Modules.DocumentReview.Domain.Aggregates;
using ARAP.Modules.DocumentReview.Domain.Repositories;
using ARAP.Modules.DocumentReview.Domain.ValueObjects;
using ARAP.Modules.DocumentReview.Infrastructure.Persistence;

namespace ARAP.Modules.DocumentReview.Infrastructure.Persistence.Repositories;

public sealed class DocumentRepository : IDocumentRepository
{
    private readonly DocumentReviewDbContext _context;

    public DocumentRepository(DocumentReviewDbContext context)
    {
        _context = context;
    }

    public async Task<Document?> GetByIdAsync(DocumentId id, CancellationToken cancellationToken = default)
    {
        return await _context.Documents
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Document>> GetByProposalIdAsync(Guid proposalId, CancellationToken cancellationToken = default)
    {
        return await _context.Documents
            .Where(d => d.ProposalId == proposalId)
            .OrderByDescending(d => d.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Document>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _context.Documents
            .Where(d => d.StudentId == studentId)
            .OrderByDescending(d => d.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Document>> GetByReviewerIdAsync(Guid reviewerId, CancellationToken cancellationToken = default)
    {
        return await _context.Documents
            .Where(d => d.ReviewerId == reviewerId)
            .OrderByDescending(d => d.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Document>> GetByStatusAsync(ReviewStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Documents
            .Where(d => d.Status == status)
            .OrderByDescending(d => d.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Document document, CancellationToken cancellationToken = default)
    {
        await _context.Documents.AddAsync(document, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Document document, CancellationToken cancellationToken = default)
    {
        _context.Documents.Update(document);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Document document, CancellationToken cancellationToken = default)
    {
        _context.Documents.Remove(document);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
