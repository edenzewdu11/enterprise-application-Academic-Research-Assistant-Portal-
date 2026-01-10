using ARAP.SharedKernel;
using ARAP.Modules.ProgressTracking.Domain.ValueObjects;
using ARAP.Modules.ProgressTracking.Domain.Events;

namespace ARAP.Modules.ProgressTracking.Domain.Aggregates;

/// <summary>
/// ActivityLog aggregate root - tracks research activities and progress
/// </summary>
public sealed class ActivityLog : AggregateRoot<ActivityLogId>
{
    public Guid ProposalId { get; private set; }
    public Guid StudentId { get; private set; }
    public ActivityType Type { get; private set; }
    public ActivityDescription Description { get; private set; }
    public ProgressPercentage Progress { get; private set; }
    public DateTime LoggedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public int HoursSpent { get; private set; }

    // EF Core constructor
    private ActivityLog() : base(ActivityLogId.CreateUnique()) { }

    private ActivityLog(
        ActivityLogId id,
        Guid proposalId,
        Guid studentId,
        ActivityType type,
        ActivityDescription description,
        ProgressPercentage progress,
        int hoursSpent)
        : base(id)
    {
        ProposalId = proposalId;
        StudentId = studentId;
        Type = type;
        Description = description;
        Progress = progress;
        HoursSpent = hoursSpent;
        LoggedAt = DateTime.UtcNow;
    }

    public static Result<ActivityLog> Create(
        Guid proposalId,
        Guid studentId,
        ActivityType type,
        ActivityDescription description,
        ProgressPercentage progress,
        int hoursSpent = 0)
    {
        if (proposalId == Guid.Empty)
            return Result.Failure<ActivityLog>("Proposal ID cannot be empty");

        if (studentId == Guid.Empty)
            return Result.Failure<ActivityLog>("Student ID cannot be empty");

        if (hoursSpent < 0)
            return Result.Failure<ActivityLog>("Hours spent cannot be negative");

        var activity = new ActivityLog(
            ActivityLogId.CreateUnique(),
            proposalId,
            studentId,
            type,
            description,
            progress,
            hoursSpent);

        activity.RaiseDomainEvent(new ActivityLoggedEvent(
            activity.Id.Value,
            proposalId,
            studentId,
            type.Name,
            description.Value,
            activity.LoggedAt));

        return Result.Success(activity);
    }

    public Result UpdateProgress(ProgressPercentage newProgress)
    {
        if (CompletedAt.HasValue)
            return Result.Failure("Cannot update progress for completed activity");

        var oldPercentage = Progress.Value;
        Progress = newProgress;

        if (newProgress.IsComplete)
        {
            CompletedAt = DateTime.UtcNow;
        }

        RaiseDomainEvent(new ProgressUpdatedEvent(
            Id.Value,
            ProposalId,
            oldPercentage,
            newProgress.Value,
            DateTime.UtcNow));

        return Result.Success();
    }

    public Result AddHours(int hours)
    {
        if (hours <= 0)
            return Result.Failure("Hours must be positive");

        HoursSpent += hours;
        return Result.Success();
    }

    public Result Complete()
    {
        if (CompletedAt.HasValue)
            return Result.Failure("Activity already completed");

        CompletedAt = DateTime.UtcNow;
        
        var fullProgress = ProgressPercentage.Create(100);
        if (fullProgress.IsSuccess)
        {
            Progress = fullProgress.Value!;
        }

        return Result.Success();
    }
}
