namespace ARAP.SharedKernel;

/// <summary>
/// Base class for value objects implementing equality based on values
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Gets the components that define equality for this value object
    /// </summary>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    #region Equality

    public bool Equals(ValueObject? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueObject other && Equals(other);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Where(x => x != null)
            .Select(x => x!.GetHashCode())
            .Aggregate((x, y) => x ^ y);
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }

    #endregion

    /// <summary>
    /// Creates a copy of this value object
    /// </summary>
    public ValueObject Copy() => this;

    /// <summary>
    /// Returns string representation of the value object
    /// </summary>
    public override string ToString()
    {
        return $"{GetType().Name} {{ {string.Join(", ", GetEqualityComponents())} }}";
    }
}
