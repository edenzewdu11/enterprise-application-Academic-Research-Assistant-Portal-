using ARAP.SharedKernel;

namespace ARAP.Modules.AcademicIntegrity.Domain.Events;

/// <summary>
/// Event raised when a plagiarism check is initiated
/// </summary>
public sealed record CheckInitiatedEvent(
    Guid CheckId,
    Guid DocumentId,
    Guid ProposalId,
    DateTime InitiatedAt) : DomainEvent;
