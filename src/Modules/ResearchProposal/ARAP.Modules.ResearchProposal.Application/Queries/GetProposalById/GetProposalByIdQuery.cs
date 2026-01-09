using MediatR;
using ARAP.Modules.ResearchProposal.Application.DTOs;

namespace ARAP.Modules.ResearchProposal.Application.Queries.GetProposalById;

/// <summary>
/// Query to get a research proposal by ID
/// </summary>
public sealed record GetProposalByIdQuery : IRequest<ProposalDto?>
{
    public Guid ProposalId { get; init; }
}
