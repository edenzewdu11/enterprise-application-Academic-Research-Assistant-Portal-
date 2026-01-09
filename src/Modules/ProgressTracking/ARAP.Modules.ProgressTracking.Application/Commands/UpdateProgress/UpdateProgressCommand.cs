using MediatR;

namespace ARAP.Modules.ProgressTracking.Application.Commands.UpdateProgress;

public sealed record UpdateProgressCommand(
    Guid ActivityLogId,
    int NewProgressPercentage) : IRequest<Unit>;
