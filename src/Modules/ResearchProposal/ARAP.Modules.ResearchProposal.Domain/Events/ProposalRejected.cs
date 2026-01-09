using ARAP.SharedKernel;

namespace ARAP.Modules.ResearchProposal.Domain.Events;

/// <summary>
/// Event raised when a research proposal is rejected
/// </summary>
public sealed record ProposalRejected : DomainEvent
{
    public Guid ProposalId { get; init; }
    public Guid AdvisorId { get; init; }
    public string Reason { get; init; }
    public DateTime RejectedAt { get; init; }

    public ProposalRejected(
        Guid proposalId,
        Guid advisorId,
        string reason,
        DateTime rejectedAt)
    {
        ProposalId = proposalId;
        AdvisorId = advisorId;
        Reason = reason;
        RejectedAt = rejectedAt;
    }
}
