using ARAP.SharedKernel;

namespace ARAP.Modules.DocumentReview.Domain.Events;

public sealed record ReviewerAssignedEvent(
    Guid DocumentId,
    Guid ReviewerId,
    DateTime AssignedAt) : DomainEvent;
