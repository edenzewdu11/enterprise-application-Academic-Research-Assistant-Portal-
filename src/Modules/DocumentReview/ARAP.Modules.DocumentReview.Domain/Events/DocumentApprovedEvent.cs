using ARAP.SharedKernel;

namespace ARAP.Modules.DocumentReview.Domain.Events;

public sealed record DocumentApprovedEvent(
    Guid DocumentId,
    Guid ReviewerId,
    DateTime ApprovedAt) : DomainEvent;
