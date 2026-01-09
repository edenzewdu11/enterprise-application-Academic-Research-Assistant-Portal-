using ARAP.SharedKernel;
using ARAP.Modules.ResearchProposal.Domain.Entities;
using ARAP.Modules.ResearchProposal.Domain.Events;
using ARAP.Modules.ResearchProposal.Domain.ValueObjects;

namespace ARAP.Modules.ResearchProposal.Domain.Aggregates;

/// <summary>
/// Research Proposal Aggregate Root
/// Represents a student's research proposal throughout its lifecycle
/// </summary>
public sealed class ResearchProposal : AggregateRoot<ResearchProposalId>
{
    private readonly List<Milestone> _milestones = new();

    public Guid StudentId { get; private set; }
    public Guid AdvisorId { get; private set; }
    public ProposalTitle Title { get; private set; }
    public Abstract Abstract { get; private set; }
    public ResearchQuestion ResearchQuestion { get; private set; }
    public ProposalState State { get; private set; }
    public string? ReviewComments { get; private set; }
    public DateTime? SubmittedAt { get; private set; }
    public DateTime? ReviewedAt { get; private set; }
    
    public IReadOnlyCollection<Milestone> Milestones => _milestones.AsReadOnly();

    // EF Core constructor
    private ResearchProposal() : base(ResearchProposalId.CreateUnique()) { }

    private ResearchProposal(
        ResearchProposalId id,
        Guid studentId,
        Guid advisorId,
        ProposalTitle title,
        Abstract @abstract,
        ResearchQuestion researchQuestion)
        : base(id)
    {
        StudentId = studentId;
        AdvisorId = advisorId;
        Title = title;
        Abstract = @abstract;
        ResearchQuestion = researchQuestion;
        State = ProposalState.Draft;
    }

    public static Result<ResearchProposal> Create(
        Guid studentId,
        Guid advisorId,
        ProposalTitle title,
        Abstract @abstract,
        ResearchQuestion researchQuestion)
    {
        if (studentId == Guid.Empty)
            return Result.Failure<ResearchProposal>("Student ID cannot be empty");

        if (advisorId == Guid.Empty)
            return Result.Failure<ResearchProposal>("Advisor ID cannot be empty");

        var proposal = new ResearchProposal(
            ResearchProposalId.CreateUnique(),
            studentId,
            advisorId,
            title,
            @abstract,
            researchQuestion);

        return Result.Success(proposal);
    }

    public Result Submit()
    {
        if (!State.IsDraft)
            return Result.Failure("Only draft proposals can be submitted");

        if (!_milestones.Any())
            return Result.Failure("Cannot submit proposal without milestones");

        State = ProposalState.Submitted;
        SubmittedAt = DateTime.UtcNow;
        MarkAsModified();

        RaiseDomainEvent(new ProposalSubmitted(
            Id.Value,
            StudentId,
            Title.Value,
            AdvisorId,
            SubmittedAt.Value));

        return Result.Success();
    }

    public Result Approve(Guid advisorId, string comments)
    {
        if (advisorId != AdvisorId)
            return Result.Failure("Only assigned advisor can approve the proposal");

        if (!State.IsSubmitted && !State.IsUnderReview)
            return Result.Failure("Only submitted or under-review proposals can be approved");

        State = ProposalState.Approved;
        ReviewComments = comments;
        ReviewedAt = DateTime.UtcNow;
        MarkAsModified();

        RaiseDomainEvent(new ProposalApproved(
            Id.Value,
            advisorId,
            comments,
            ReviewedAt.Value));

        return Result.Success();
    }

    public Result Reject(Guid advisorId, string reason)
    {
        if (advisorId != AdvisorId)
            return Result.Failure("Only assigned advisor can reject the proposal");

        if (!State.IsSubmitted && !State.IsUnderReview)
            return Result.Failure("Only submitted or under-review proposals can be rejected");

        if (string.IsNullOrWhiteSpace(reason))
            return Result.Failure("Rejection reason is required");

        State = ProposalState.Rejected;
        ReviewComments = reason;
        ReviewedAt = DateTime.UtcNow;
        MarkAsModified();

        RaiseDomainEvent(new ProposalRejected(
            Id.Value,
            advisorId,
            reason,
            ReviewedAt.Value));

        return Result.Success();
    }

    public Result RequestRevision(Guid advisorId, string feedback)
    {
        if (advisorId != AdvisorId)
            return Result.Failure("Only assigned advisor can request revisions");

        if (!State.IsSubmitted && !State.IsUnderReview)
            return Result.Failure("Only submitted or under-review proposals can have revisions requested");

        if (string.IsNullOrWhiteSpace(feedback))
            return Result.Failure("Feedback is required when requesting revision");

        State = ProposalState.RevisionRequired;
        ReviewComments = feedback;
        ReviewedAt = DateTime.UtcNow;
        MarkAsModified();

        RaiseDomainEvent(new RevisionRequested(
            Id.Value,
            advisorId,
            feedback,
            ReviewedAt.Value));

        return Result.Success();
    }

    public Result AddMilestone(Milestone milestone)
    {
        if (!State.IsDraft)
            return Result.Failure("Can only add milestones to draft proposals");

        if (_milestones.Any(m => m.Type == milestone.Type))
            return Result.Failure($"Milestone of type {milestone.Type} already exists");

        _milestones.Add(milestone);
        MarkAsModified();

        return Result.Success();
    }

    public Result CompleteMilestone(MilestoneId milestoneId)
    {
        if (!State.IsApproved)
            return Result.Failure("Can only complete milestones on approved proposals");

        var milestone = _milestones.FirstOrDefault(m => m.Id == milestoneId);
        if (milestone is null)
            return Result.Failure("Milestone not found");

        var result = milestone.Complete();
        if (result.IsFailure)
            return result;

        MarkAsModified();

        RaiseDomainEvent(new MilestoneCompleted(
            Id.Value,
            milestoneId.Value,
            milestone.Type.Value,
            DateTime.UtcNow));

        return Result.Success();
    }

    public void UpdateTitle(ProposalTitle newTitle)
    {
        if (State.IsFinal)
            throw new DomainException("Cannot update title of finalized proposal");

        Title = newTitle;
        MarkAsModified();
    }

    public void UpdateAbstract(Abstract newAbstract)
    {
        if (State.IsFinal)
            throw new DomainException("Cannot update abstract of finalized proposal");

        Abstract = newAbstract;
        MarkAsModified();
    }

    public void UpdateResearchQuestion(ResearchQuestion newQuestion)
    {
        if (State.IsFinal)
            throw new DomainException("Cannot update research question of finalized proposal");

        ResearchQuestion = newQuestion;
        MarkAsModified();
    }
}
