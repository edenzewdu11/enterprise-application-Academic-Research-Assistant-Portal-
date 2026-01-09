using ARAP.SharedKernel;

namespace ARAP.Modules.AcademicIntegrity.Domain.Events;


public sealed record CheckCompletedEvent(
    Guid CheckId,
    Guid DocumentId,
    decimal SimilarityScore,
    bool IsHighRisk,
    DateTime CompletedAt) : DomainEvent;
