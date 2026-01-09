using ARAP.SharedKernel;

namespace ARAP.Modules.ProgressTracking.Domain.Events;

/// <summary>
/// Event raised when a research activity is logged
/// </summary>
public sealed record ActivityLoggedEvent(
    Guid ActivityLogId,
    Guid ProposalId,
    Guid StudentId,
    string ActivityType,
    string Description,
    DateTime LoggedAt) : DomainEvent;
