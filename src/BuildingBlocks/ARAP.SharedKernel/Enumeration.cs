namespace ARAP.SharedKernel;

/// <summary>
/// Base class for enumeration types with value-based equality
/// </summary>
public abstract class Enumeration : IEquatable<Enumeration>, IComparable<Enumeration>
{
    /// <summary>
    /// Gets the integer value of the enumeration
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Gets the name of the enumeration
    /// </summary>
    public string Name { get; }

    protected Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public override string ToString() => Name;

    /// <summary>
    /// Gets all instances of the enumeration type
    /// </summary>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var fields = typeof(T).GetFields(System.Reflection.BindingFlags.Public |
                                         System.Reflection.BindingFlags.Static |
                                         System.Reflection.BindingFlags.DeclaredOnly);

        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
            return false;

        var typeMatches = GetType() == obj.GetType();
        var valueMatches = Value.Equals(otherValue.Value);

        return typeMatches && valueMatches;
    }

    public bool Equals(Enumeration? other)
    {
        if (other is null)
            return false;

        return GetType() == other.GetType() && Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public int CompareTo(Enumeration? other) => Value.CompareTo(other?.Value);

    public static bool operator ==(Enumeration? left, Enumeration? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Enumeration? left, Enumeration? right) => !(left == right);

    public static bool operator <(Enumeration left, Enumeration right) => left.CompareTo(right) < 0;

    public static bool operator <=(Enumeration left, Enumeration right) => left.CompareTo(right) <= 0;

    public static bool operator >(Enumeration left, Enumeration right) => left.CompareTo(right) > 0;

    public static bool operator >=(Enumeration left, Enumeration right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Gets enumeration instance by value
    /// </summary>
    public static T? FromValue<T>(int value) where T : Enumeration
    {
        return GetAll<T>().FirstOrDefault(e => e.Value == value);
    }

    /// <summary>
    /// Gets enumeration instance by name
    /// </summary>
    public static T? FromName<T>(string name) where T : Enumeration
    {
        return GetAll<T>().FirstOrDefault(e => string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase));
    }
}
