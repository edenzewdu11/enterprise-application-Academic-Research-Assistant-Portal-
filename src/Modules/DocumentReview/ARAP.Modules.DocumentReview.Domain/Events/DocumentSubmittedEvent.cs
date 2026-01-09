using ARAP.SharedKernel;

namespace ARAP.Modules.DocumentReview.Domain.Events;

public sealed record DocumentSubmittedEvent(
    Guid DocumentId,
    Guid ProposalId,
    Guid StudentId,
    string DocumentType,
    string FileName,
    DateTime SubmittedAt) : DomainEvent;
