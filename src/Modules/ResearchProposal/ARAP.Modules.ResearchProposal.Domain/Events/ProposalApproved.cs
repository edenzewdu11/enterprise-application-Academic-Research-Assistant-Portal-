using ARAP.SharedKernel;

namespace ARAP.Modules.ResearchProposal.Domain.Events;

/// <summary>
/// Event raised when a research proposal is approved by an advisor
/// </summary>
public sealed record ProposalApproved : DomainEvent
{
    public Guid ProposalId { get; init; }
    public Guid AdvisorId { get; init; }
    public string Comments { get; init; }
    public DateTime ApprovedAt { get; init; }

    public ProposalApproved(
        Guid proposalId,
        Guid advisorId,
        string comments,
        DateTime approvedAt)
    {
        ProposalId = proposalId;
        AdvisorId = advisorId;
        Comments = comments;
        ApprovedAt = approvedAt;
    }
}
