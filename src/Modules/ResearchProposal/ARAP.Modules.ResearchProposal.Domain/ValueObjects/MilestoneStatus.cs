using ARAP.SharedKernel;

namespace ARAP.Modules.ResearchProposal.Domain.ValueObjects;

/// <summary>
/// Represents the status of a milestone
/// </summary>
public sealed class MilestoneStatus : ValueObject
{
    public string Value { get; }

    public static readonly MilestoneStatus Pending = new("Pending");
    public static readonly MilestoneStatus InProgress = new("InProgress");
    public static readonly MilestoneStatus Completed = new("Completed");
    public static readonly MilestoneStatus Overdue = new("Overdue");

    private MilestoneStatus(string value)
    {
        Value = value;
    }

    public static MilestoneStatus FromString(string value)
    {
        return value switch
        {
            "Pending" => Pending,
            "InProgress" => InProgress,
            "Completed" => Completed,
            "Overdue" => Overdue,
            _ => throw new ArgumentException($"Invalid milestone status: {value}", nameof(value))
        };
    }

    public bool IsPending => this == Pending;
    public bool IsInProgress => this == InProgress;
    public bool IsCompleted => this == Completed;
    public bool IsOverdue => this == Overdue;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
