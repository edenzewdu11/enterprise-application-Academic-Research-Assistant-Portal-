using MediatR;

namespace ARAP.Modules.ResearchProposal.Application.Commands.ApproveProposal;


public sealed record ApproveProposalCommand : IRequest<Unit>
{
    public Guid ProposalId { get; init; }
    public Guid AdvisorId { get; init; }
    public string Comments { get; init; } = string.Empty;
}
