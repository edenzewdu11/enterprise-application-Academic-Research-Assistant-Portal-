using ARAP.Modules.ProgressTracking.Domain.Aggregates;
using ARAP.Modules.ProgressTracking.Domain.ValueObjects;

namespace ARAP.Modules.ProgressTracking.Domain.Repositories;

/// <summary>
/// Repository interface for ActivityLog aggregate
/// </summary>
public interface IActivityLogRepository
{
    Task<ActivityLog?> GetByIdAsync(ActivityLogId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActivityLog>> GetByProposalIdAsync(Guid proposalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActivityLog>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActivityLog>> GetRecentActivitiesAsync(Guid proposalId, int count, CancellationToken cancellationToken = default);
    Task AddAsync(ActivityLog activityLog, CancellationToken cancellationToken = default);
    Task UpdateAsync(ActivityLog activityLog, CancellationToken cancellationToken = default);
    Task<int> GetTotalHoursAsync(Guid proposalId, CancellationToken cancellationToken = default);
}
