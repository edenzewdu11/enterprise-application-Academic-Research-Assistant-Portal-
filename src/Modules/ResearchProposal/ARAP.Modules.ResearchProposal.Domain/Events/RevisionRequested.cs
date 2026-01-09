using ARAP.SharedKernel;

namespace ARAP.Modules.ResearchProposal.Domain.Events;

/// <summary>
/// Event raised when revision is requested for a research proposal
/// </summary>
public sealed record RevisionRequested : DomainEvent
{
    public Guid ProposalId { get; init; }
    public Guid AdvisorId { get; init; }
    public string Feedback { get; init; }
    public DateTime RequestedAt { get; init; }

    public RevisionRequested(
        Guid proposalId,
        Guid advisorId,
        string feedback,
        DateTime requestedAt)
    {
        ProposalId = proposalId;
        AdvisorId = advisorId;
        Feedback = feedback;
        RequestedAt = requestedAt;
    }
}
