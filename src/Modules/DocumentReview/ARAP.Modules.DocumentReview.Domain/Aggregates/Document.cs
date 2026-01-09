using ARAP.SharedKernel;
using ARAP.Modules.DocumentReview.Domain.ValueObjects;
using ARAP.Modules.DocumentReview.Domain.Events;

namespace ARAP.Modules.DocumentReview.Domain.Aggregates;

/// <summary>
/// Document aggregate root - represents a research document submitted for review
/// </summary>
public sealed class Document : AggregateRoot<DocumentId>
{
    public Guid ProposalId { get; private set; }
    public Guid StudentId { get; private set; }
    public DocumentType Type { get; private set; }
    public string FileName { get; private set; }
    public string FileUrl { get; private set; }
    public ReviewStatus Status { get; private set; }
    public int DocumentVersion { get; private set; }
    public DateTime SubmittedAt { get; private set; }
    public DateTime? ReviewedAt { get; private set; }
    public Guid? ReviewerId { get; private set; }
    public FeedbackComment? Feedback { get; private set; }

    // EF Core constructor
    private Document() : base(DocumentId.CreateUnique()) { }

    private Document(
        DocumentId id,
        Guid proposalId,
        Guid studentId,
        DocumentType type,
        string fileName,
        string fileUrl,
        int version)
        : base(id)
    {
        ProposalId = proposalId;
        StudentId = studentId;
        Type = type;
        FileName = fileName;
        FileUrl = fileUrl;
        Status = ReviewStatus.Pending;
        DocumentVersion = version;
        SubmittedAt = DateTime.UtcNow;
    }

    public static Result<Document> Create(
        Guid proposalId,
        Guid studentId,
        DocumentType type,
        string fileName,
        string fileUrl,
        int version = 1)
    {
        if (proposalId == Guid.Empty)
            return Result.Failure<Document>("Proposal ID cannot be empty");

        if (studentId == Guid.Empty)
            return Result.Failure<Document>("Student ID cannot be empty");

        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Failure<Document>("File name cannot be empty");

        if (string.IsNullOrWhiteSpace(fileUrl))
            return Result.Failure<Document>("File URL cannot be empty");

        if (version < 1)
            return Result.Failure<Document>("Version must be greater than 0");

        var document = new Document(
            DocumentId.CreateUnique(),
            proposalId,
            studentId,
            type,
            fileName.Trim(),
            fileUrl.Trim(),
            version);

        document.RaiseDomainEvent(new DocumentSubmittedEvent(
            document.Id.Value,
            proposalId,
            studentId,
            type.Value,
            fileName,
            document.SubmittedAt));

        return Result.Success(document);
    }

    public Result AssignReviewer(Guid reviewerId)
    {
        if (reviewerId == Guid.Empty)
            return Result.Failure("Reviewer ID cannot be empty");

        if (!Status.IsPending)
            return Result.Failure("Can only assign reviewer to pending documents");

        ReviewerId = reviewerId;
        Status = ReviewStatus.InReview;

        RaiseDomainEvent(new ReviewerAssignedEvent(
            Id.Value,
            reviewerId,
            DateTime.UtcNow));

        return Result.Success();
    }

    public Result ProvideFeedback(Guid reviewerId, FeedbackComment comment)
    {
        if (ReviewerId != reviewerId)
            return Result.Failure("Only assigned reviewer can provide feedback");

        if (!Status.IsInReview)
            return Result.Failure("Document must be in review to provide feedback");

        Feedback = comment;
        Status = ReviewStatus.ChangesRequested;
        ReviewedAt = DateTime.UtcNow;

        RaiseDomainEvent(new FeedbackProvidedEvent(
            Id.Value,
            reviewerId,
            comment.Value,
            DateTime.UtcNow));

        return Result.Success();
    }

    public Result Approve(Guid reviewerId)
    {
        if (ReviewerId != reviewerId)
            return Result.Failure("Only assigned reviewer can approve document");

        if (!Status.IsInReview)
            return Result.Failure("Document must be in review to approve");

        Status = ReviewStatus.Approved;
        ReviewedAt = DateTime.UtcNow;

        RaiseDomainEvent(new DocumentApprovedEvent(
            Id.Value,
            reviewerId,
            DateTime.UtcNow));

        return Result.Success();
    }

    public Result Reject(Guid reviewerId, FeedbackComment reason)
    {
        if (ReviewerId != reviewerId)
            return Result.Failure("Only assigned reviewer can reject document");

        if (!Status.IsInReview)
            return Result.Failure("Document must be in review to reject");

        Feedback = reason;
        Status = ReviewStatus.Rejected;
        ReviewedAt = DateTime.UtcNow;

        RaiseDomainEvent(new DocumentRejectedEvent(
            Id.Value,
            reviewerId,
            reason.Value,
            DateTime.UtcNow));

        return Result.Failure("Document rejected");
    }
}
