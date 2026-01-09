using MediatR;

namespace ARAP.Modules.ResearchProposal.Application.Commands.SubmitProposal;

/// <summary>
/// Command to submit a proposal for review
/// </summary>
public sealed record SubmitProposalCommand : IRequest<Unit>
{
    public Guid ProposalId { get; init; }
    public Guid StudentId { get; init; }
}
