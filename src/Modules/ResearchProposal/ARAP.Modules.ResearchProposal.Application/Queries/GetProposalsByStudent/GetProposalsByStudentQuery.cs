using MediatR;
using ARAP.Modules.ResearchProposal.Application.DTOs;

namespace ARAP.Modules.ResearchProposal.Application.Queries.GetProposalsByStudent;

/// <summary>
/// Query to get all proposals for a student
/// </summary>
public sealed record GetProposalsByStudentQuery : IRequest<List<ProposalDto>>
{
    public Guid StudentId { get; init; }
}
