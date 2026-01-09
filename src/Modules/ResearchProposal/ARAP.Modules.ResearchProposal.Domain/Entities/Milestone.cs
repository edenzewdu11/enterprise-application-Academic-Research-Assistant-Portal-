using ARAP.SharedKernel;
using ARAP.Modules.ResearchProposal.Domain.ValueObjects;

namespace ARAP.Modules.ResearchProposal.Domain.Entities;

/// <summary>
/// Represents a milestone in the research proposal lifecycle
/// </summary>
public sealed class Milestone : Entity<MilestoneId>
{
    public MilestoneType Type { get; private set; }
    public string Description { get; private set; }
    public DateTime Deadline { get; private set; }
    public DateTime? CompletionDate { get; private set; }
    public MilestoneStatus Status { get; private set; }

    // EF Core constructor
    private Milestone() : base(MilestoneId.CreateUnique()) { }

    private Milestone(
        MilestoneId id,
        MilestoneType type,
        string description,
        DateTime deadline)
        : base(id)
    {
        Type = type;
        Description = description;
        Deadline = deadline;
        Status = MilestoneStatus.Pending;
    }

    public static Result<Milestone> Create(
        MilestoneType type,
        string description,
        DateTime deadline)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Milestone>("Milestone description cannot be empty");

        if (description.Length > 500)
            return Result.Failure<Milestone>("Milestone description cannot exceed 500 characters");

        if (deadline <= DateTime.UtcNow)
            return Result.Failure<Milestone>("Milestone deadline must be in the future");

        var milestone = new Milestone(
            MilestoneId.CreateUnique(),
            type,
            description.Trim(),
            deadline);

        return Result.Success(milestone);
    }

    public Result MarkAsInProgress()
    {
        if (Status.IsCompleted)
            return Result.Failure("Cannot change status of completed milestone");

        if (Status.IsInProgress)
            return Result.Failure("Milestone is already in progress");

        Status = MilestoneStatus.InProgress;
        return Result.Success();
    }

    public Result Complete()
    {
        if (Status.IsCompleted)
            return Result.Failure("Milestone is already completed");

        Status = MilestoneStatus.Completed;
        CompletionDate = DateTime.UtcNow;
        return Result.Success();
    }

    public void UpdateDeadline(DateTime newDeadline)
    {
        if (Status.IsCompleted)
            throw new DomainException("Cannot update deadline of completed milestone");

        if (newDeadline <= DateTime.UtcNow)
            throw new DomainException("New deadline must be in the future");

        Deadline = newDeadline;
    }

    public void CheckIfOverdue()
    {
        if (!Status.IsCompleted && DateTime.UtcNow > Deadline)
        {
            Status = MilestoneStatus.Overdue;
        }
    }
}
