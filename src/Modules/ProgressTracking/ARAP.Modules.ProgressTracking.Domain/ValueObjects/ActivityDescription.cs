using ARAP.SharedKernel;

namespace ARAP.Modules.ProgressTracking.Domain.ValueObjects;

/// <summary>
/// Value object for activity description with validation
/// </summary>
public sealed class ActivityDescription : ValueObject
{
    public string Value { get; }

    private ActivityDescription(string value)
    {
        Value = value;
    }

    public static Result<ActivityDescription> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<ActivityDescription>("Activity description cannot be empty");

        var trimmed = value.Trim();
        
        if (trimmed.Length < 10)
            return Result.Failure<ActivityDescription>("Activity description must be at least 10 characters");

        if (trimmed.Length > 1000)
            return Result.Failure<ActivityDescription>("Activity description cannot exceed 1000 characters");

        return Result.Success(new ActivityDescription(trimmed));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
