using ARAP.SharedKernel;
using ARAP.Modules.AcademicIntegrity.Domain.ValueObjects;
using ARAP.Modules.AcademicIntegrity.Domain.Events;

namespace ARAP.Modules.AcademicIntegrity.Domain.Aggregates;

/// <summary>
/// Plagiarism check aggregate root
/// </summary>
public sealed class PlagiarismCheck : AggregateRoot<PlagiarismCheckId>
{
    public Guid DocumentId { get; private set; }
    public Guid ProposalId { get; private set; }
    public Guid InitiatedBy { get; private set; }
    public CheckStatus Status { get; private set; }
    public SimilarityScore? Score { get; private set; }
    public string? ExternalCheckId { get; private set; }
    public CheckNotes? Notes { get; private set; }
    public DateTime InitiatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // EF Core constructor
    private PlagiarismCheck() : base(PlagiarismCheckId.CreateUnique()) { }

    private PlagiarismCheck(
        PlagiarismCheckId id,
        Guid documentId,
        Guid proposalId,
        Guid initiatedBy)
        : base(id)
    {
        DocumentId = documentId;
        ProposalId = proposalId;
        InitiatedBy = initiatedBy;
        Status = CheckStatus.Pending;
        InitiatedAt = DateTime.UtcNow;
    }

    public static Result<PlagiarismCheck> Create(
        Guid documentId,
        Guid proposalId,
        Guid initiatedBy)
    {
        if (documentId == Guid.Empty)
            return Result.Failure<PlagiarismCheck>("Document ID cannot be empty");

        if (proposalId == Guid.Empty)
            return Result.Failure<PlagiarismCheck>("Proposal ID cannot be empty");

        if (initiatedBy == Guid.Empty)
            return Result.Failure<PlagiarismCheck>("Initiator ID cannot be empty");

        var check = new PlagiarismCheck(
            PlagiarismCheckId.CreateUnique(),
            documentId,
            proposalId,
            initiatedBy);

        check.RaiseDomainEvent(new CheckInitiatedEvent(
            check.Id.Value,
            documentId,
            proposalId,
            check.InitiatedAt));

        return Result.Success(check);
    }

    public Result StartProcessing(string externalCheckId)
    {
        if (!Status.IsPending)
            return Result.Failure("Can only start processing a pending check");

        if (string.IsNullOrWhiteSpace(externalCheckId))
            return Result.Failure("External check ID is required");

        Status = CheckStatus.InProgress;
        ExternalCheckId = externalCheckId.Trim();

        return Result.Success();
    }

    public Result Complete(SimilarityScore score, CheckNotes? notes = null)
    {
        if (!Status.IsInProgress && !Status.IsPending)
            return Result.Failure("Can only complete a check that is pending or in progress");

        Status = CheckStatus.Completed;
        Score = score;
        Notes = notes;
        CompletedAt = DateTime.UtcNow;

        RaiseDomainEvent(new CheckCompletedEvent(
            Id.Value,
            DocumentId,
            score.Value,
            score.IsHighRisk,
            CompletedAt.Value));

        return Result.Success();
    }

    public Result Fail(CheckNotes reason)
    {
        if (Status.IsCompleted)
            return Result.Failure("Cannot fail a completed check");

        Status = CheckStatus.Failed;
        Notes = reason;
        CompletedAt = DateTime.UtcNow;

        return Result.Success();
    }
}
