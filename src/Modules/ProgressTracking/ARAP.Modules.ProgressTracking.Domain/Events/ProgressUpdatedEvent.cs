using ARAP.SharedKernel;

namespace ARAP.Modules.ProgressTracking.Domain.Events;

/// <summary>
/// Event raised when progress percentage is updated
/// </summary>
public sealed record ProgressUpdatedEvent(
    Guid ActivityLogId,
    Guid ProposalId,
    int OldPercentage,
    int NewPercentage,
    DateTime UpdatedAt) : DomainEvent;
