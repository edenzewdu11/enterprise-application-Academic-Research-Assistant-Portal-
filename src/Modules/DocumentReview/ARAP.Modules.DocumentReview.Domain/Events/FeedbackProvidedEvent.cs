using ARAP.SharedKernel;

namespace ARAP.Modules.DocumentReview.Domain.Events;

public sealed record FeedbackProvidedEvent(
    Guid DocumentId,
    Guid ReviewerId,
    string Comment,
    DateTime ProvidedAt) : DomainEvent;
