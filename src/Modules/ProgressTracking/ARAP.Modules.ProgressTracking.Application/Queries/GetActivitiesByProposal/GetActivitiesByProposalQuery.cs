using MediatR;
using ARAP.Modules.ProgressTracking.Application.DTOs;

namespace ARAP.Modules.ProgressTracking.Application.Queries.GetActivitiesByProposal;

public sealed record GetActivitiesByProposalQuery(Guid ProposalId) : IRequest<IReadOnlyList<ActivityLogDto>>;
