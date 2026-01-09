using MediatR;

namespace ARAP.Modules.ResearchProposal.Application.Commands.SubmitProposal;


public sealed record SubmitProposalCommand : IRequest<Unit>
{
    public Guid ProposalId { get; init; }
    public Guid StudentId { get; init; }
}
