using MediatR;

namespace ARAP.Modules.ProgressTracking.Application.Commands.LogActivity;

public sealed record LogActivityCommand(
    Guid ProposalId,
    Guid StudentId,
    string ActivityType,
    string Description,
    int ProgressPercentage,
    int HoursSpent) : IRequest<Guid>;
