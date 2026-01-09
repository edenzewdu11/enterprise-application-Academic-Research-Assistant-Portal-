using MediatR;
using ARAP.Modules.ProgressTracking.Application.DTOs;

namespace ARAP.Modules.ProgressTracking.Application.Queries.GetProgressSummary;

public sealed record GetProgressSummaryQuery(Guid ProposalId) : IRequest<ProgressSummaryDto>;
