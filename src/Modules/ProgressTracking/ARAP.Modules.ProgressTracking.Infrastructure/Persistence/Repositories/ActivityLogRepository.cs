using Microsoft.EntityFrameworkCore;
using ARAP.Modules.ProgressTracking.Domain.Aggregates;
using ARAP.Modules.ProgressTracking.Domain.Repositories;
using ARAP.Modules.ProgressTracking.Domain.ValueObjects;
using ARAP.Modules.ProgressTracking.Infrastructure.Persistence;

namespace ARAP.Modules.ProgressTracking.Infrastructure.Persistence.Repositories;

internal sealed class ActivityLogRepository : IActivityLogRepository
{
    private readonly ProgressTrackingDbContext _context;

    public ActivityLogRepository(ProgressTrackingDbContext context)
    {
        _context = context;
    }

    public async Task<ActivityLog?> GetByIdAsync(ActivityLogId id, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ActivityLog>> GetByProposalIdAsync(
        Guid proposalId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.ProposalId == proposalId)
            .OrderByDescending(a => a.LoggedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ActivityLog>> GetByStudentIdAsync(
        Guid studentId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.StudentId == studentId)
            .OrderByDescending(a => a.LoggedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ActivityLog>> GetRecentActivitiesAsync(
        Guid proposalId, 
        int count, 
        CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.ProposalId == proposalId)
            .OrderByDescending(a => a.LoggedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ActivityLog activityLog, CancellationToken cancellationToken = default)
    {
        await _context.ActivityLogs.AddAsync(activityLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ActivityLog activityLog, CancellationToken cancellationToken = default)
    {
        _context.ActivityLogs.Update(activityLog);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetTotalHoursAsync(Guid proposalId, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.ProposalId == proposalId)
            .SumAsync(a => a.HoursSpent, cancellationToken);
    }
}
