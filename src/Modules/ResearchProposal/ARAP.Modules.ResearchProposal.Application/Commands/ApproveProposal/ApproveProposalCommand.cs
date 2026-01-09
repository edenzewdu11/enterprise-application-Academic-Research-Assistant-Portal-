using MediatR;

namespace ARAP.Modules.ResearchProposal.Application.Commands.ApproveProposal;

/// <summary>
/// Command to approve a research proposal
/// </summary>
public sealed record ApproveProposalCommand : IRequest<Unit>
{
    public Guid ProposalId { get; init; }
    public Guid AdvisorId { get; init; }
    public string Comments { get; init; } = string.Empty;
}
