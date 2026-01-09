using ARAP.SharedKernel;

namespace ARAP.Modules.DocumentReview.Domain.Events;

public sealed record DocumentRejectedEvent(
    Guid DocumentId,
    Guid ReviewerId,
    string Reason,
    DateTime RejectedAt) : DomainEvent;
