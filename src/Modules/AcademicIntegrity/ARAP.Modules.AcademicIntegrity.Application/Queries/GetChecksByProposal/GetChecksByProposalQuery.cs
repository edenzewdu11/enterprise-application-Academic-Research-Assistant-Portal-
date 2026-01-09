using MediatR;
using ARAP.Modules.AcademicIntegrity.Application.DTOs;

namespace ARAP.Modules.AcademicIntegrity.Application.Queries.GetChecksByProposal;

public sealed record GetChecksByProposalQuery(Guid ProposalId) : IRequest<IReadOnlyList<PlagiarismCheckDto>>;
