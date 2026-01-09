using Microsoft.EntityFrameworkCore;
using ARAP.Modules.AcademicIntegrity.Domain.Aggregates;
using ARAP.Modules.AcademicIntegrity.Domain.Repositories;
using ARAP.Modules.AcademicIntegrity.Domain.ValueObjects;
using ARAP.Modules.AcademicIntegrity.Infrastructure.Persistence;

namespace ARAP.Modules.AcademicIntegrity.Infrastructure.Persistence.Repositories;

internal sealed class PlagiarismCheckRepository : IPlagiarismCheckRepository
{
    private readonly AcademicIntegrityDbContext _context;

    public PlagiarismCheckRepository(AcademicIntegrityDbContext context)
    {
        _context = context;
    }

    public async Task<PlagiarismCheck?> GetByIdAsync(PlagiarismCheckId id, CancellationToken cancellationToken = default)
    {
        return await _context.PlagiarismChecks
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<PlagiarismCheck?> GetByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default)
    {
        return await _context.PlagiarismChecks
            .Where(c => c.DocumentId == documentId)
            .OrderByDescending(c => c.InitiatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PlagiarismCheck>> GetByProposalIdAsync(Guid proposalId, CancellationToken cancellationToken = default)
    {
        return await _context.PlagiarismChecks
            .Where(c => c.ProposalId == proposalId)
            .OrderByDescending(c => c.InitiatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PlagiarismCheck check, CancellationToken cancellationToken = default)
    {
        await _context.PlagiarismChecks.AddAsync(check, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(PlagiarismCheck check, CancellationToken cancellationToken = default)
    {
        _context.PlagiarismChecks.Update(check);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
