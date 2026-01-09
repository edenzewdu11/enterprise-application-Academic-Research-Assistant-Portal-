using ARAP.SharedKernel;

namespace ARAP.Modules.ProgressTracking.Domain.ValueObjects;

/// <summary>
/// Value object representing progress as a percentage (0-100)
/// </summary>
public sealed class ProgressPercentage : ValueObject
{
    public int Value { get; }

    private ProgressPercentage(int value)
    {
        Value = value;
    }

    public static Result<ProgressPercentage> Create(int value)
    {
        if (value < 0 || value > 100)
            return Result.Failure<ProgressPercentage>("Progress percentage must be between 0 and 100");

        return Result.Success(new ProgressPercentage(value));
    }

    public bool IsComplete => Value == 100;
    public bool IsNotStarted => Value == 0;
    public bool IsInProgress => Value > 0 && Value < 100;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => $"{Value}%";
}
