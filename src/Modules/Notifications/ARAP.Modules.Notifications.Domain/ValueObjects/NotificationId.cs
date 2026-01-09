using ARAP.SharedKernel;

namespace ARAP.Modules.Notifications.Domain.ValueObjects;

public sealed class NotificationId : ValueObject
{
    public Guid Value { get; }

    private NotificationId(Guid value)
    {
        Value = value;
    }

    public static NotificationId New() => new(Guid.NewGuid());
    public static NotificationId From(Guid id) => new(id);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
