using ARAP.SharedKernel;

namespace ARAP.Modules.ResearchProposal.Domain.Events;

/// <summary>
/// Event raised when a milestone in a research proposal is completed
/// </summary>
public sealed record MilestoneCompleted : DomainEvent
{
    public Guid ProposalId { get; init; }
    public Guid MilestoneId { get; init; }
    public string MilestoneType { get; init; }
    public DateTime CompletedAt { get; init; }

    public MilestoneCompleted(
        Guid proposalId,
        Guid milestoneId,
        string milestoneType,
        DateTime completedAt)
    {
        ProposalId = proposalId;
        MilestoneId = milestoneId;
        MilestoneType = milestoneType;
        CompletedAt = completedAt;
    }
}
