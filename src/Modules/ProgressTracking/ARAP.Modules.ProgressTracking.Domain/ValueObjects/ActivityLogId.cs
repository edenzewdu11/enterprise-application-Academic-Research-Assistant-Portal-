using ARAP.SharedKernel;

namespace ARAP.Modules.ProgressTracking.Domain.ValueObjects;

public sealed class ActivityLogId : ValueObject
{
    public Guid Value { get; }

    private ActivityLogId(Guid value)
    {
        Value = value;
    }

    public static ActivityLogId Create(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ActivityLog ID cannot be empty", nameof(value));

        return new ActivityLogId(value);
    }

    public static ActivityLogId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
