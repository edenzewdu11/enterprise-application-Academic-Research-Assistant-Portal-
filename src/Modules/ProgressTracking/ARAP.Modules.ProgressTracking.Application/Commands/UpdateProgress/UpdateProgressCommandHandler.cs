using ARAP.Modules.ProgressTracking.Domain.Repositories;
using ARAP.Modules.ProgressTracking.Domain.ValueObjects;
using MediatR;

namespace ARAP.Modules.ProgressTracking.Application.Commands.UpdateProgress;

public sealed class UpdateProgressCommandHandler : IRequestHandler<UpdateProgressCommand, Unit>
{
    private readonly IActivityLogRepository _repository;

    public UpdateProgressCommandHandler(IActivityLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateProgressCommand request, CancellationToken cancellationToken)
    {
        var activityId = ActivityLogId.Create(request.ActivityLogId);
        var activity = await _repository.GetByIdAsync(activityId, cancellationToken);

        if (activity == null)
            throw new InvalidOperationException("Activity log not found");

        var progressResult = ProgressPercentage.Create(request.NewProgressPercentage);
        if (progressResult.IsFailure)
            throw new InvalidOperationException(progressResult.Error);

        var updateResult = activity.UpdateProgress(progressResult.Value!);
        if (updateResult.IsFailure)
            throw new InvalidOperationException(updateResult.Error);

        await _repository.UpdateAsync(activity, cancellationToken);

        return Unit.Value;
    }
}
