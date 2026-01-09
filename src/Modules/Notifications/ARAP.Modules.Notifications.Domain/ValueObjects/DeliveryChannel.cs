using ARAP.SharedKernel;

namespace ARAP.Modules.Notifications.Domain.ValueObjects;

public sealed class DeliveryChannel : Enumeration
{
    public static readonly DeliveryChannel InApp = new(1, nameof(InApp));
    public static readonly DeliveryChannel Email = new(2, nameof(Email));
    public static readonly DeliveryChannel Both = new(3, nameof(Both));

    private DeliveryChannel(int value, string name) : base(value, name)
    {
    }

    public bool IncludesInApp => Value == 1 || Value == 3;
    public bool IncludesEmail => Value == 2 || Value == 3;
}
