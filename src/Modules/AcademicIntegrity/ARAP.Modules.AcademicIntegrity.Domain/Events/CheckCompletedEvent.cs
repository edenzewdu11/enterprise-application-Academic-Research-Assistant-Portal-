using ARAP.SharedKernel;

namespace ARAP.Modules.AcademicIntegrity.Domain.Events;

/// <summary>
/// Event raised when a plagiarism check is completed
/// </summary>
public sealed record CheckCompletedEvent(
    Guid CheckId,
    Guid DocumentId,
    decimal SimilarityScore,
    bool IsHighRisk,
    DateTime CompletedAt) : DomainEvent;
