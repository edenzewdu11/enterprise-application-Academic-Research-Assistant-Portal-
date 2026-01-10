using ARAP.SharedKernel;
using ARAP.Modules.ProgressTracking.Domain.Aggregates;
using ARAP.Modules.ProgressTracking.Domain.Repositories;
using ARAP.Modules.ProgressTracking.Domain.ValueObjects;
using MediatR;

namespace ARAP.Modules.ProgressTracking.Application.Commands.LogActivity;

public sealed class LogActivityCommandHandler : IRequestHandler<LogActivityCommand, Guid>
{
    private readonly IActivityLogRepository _repository;

    public LogActivityCommandHandler(IActivityLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(LogActivityCommand request, CancellationToken cancellationToken)
    {
        // Parse activity type
        var activityType = Enumeration.FromName<ActivityType>(request.ActivityType);
        if (activityType == null)
            throw new InvalidOperationException($"Invalid activity type: {request.ActivityType}");

        // Create value objects
        var descriptionResult = ActivityDescription.Create(request.Description);
        if (descriptionResult.IsFailure)
            throw new InvalidOperationException(descriptionResult.Error);

        var progressResult = ProgressPercentage.Create(request.ProgressPercentage);
        if (progressResult.IsFailure)
            throw new InvalidOperationException(progressResult.Error);

        // Create activity log
        var activityResult = ActivityLog.Create(
            request.ProposalId,
            request.StudentId,
            activityType,
            descriptionResult.Value!,
            progressResult.Value!,
            request.HoursSpent);

        if (activityResult.IsFailure)
            throw new InvalidOperationException(activityResult.Error);

        await _repository.AddAsync(activityResult.Value!, cancellationToken);

        return activityResult.Value!.Id.Value;
    }
}
