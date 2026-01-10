namespace ARAP.SharedKernel;

/// <summary>
/// Base class for aggregate roots in the domain
/// </summary>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    /// <summary>
    /// Initializes aggregate root with ID
    /// </summary>
    protected AggregateRoot(TId id) : base(id)
    {
    }

    /// <summary>
    /// Initializes aggregate root for EF Core
    /// </summary>
    protected AggregateRoot() : base()
    {
    }

    /// <summary>
    /// Gets the version of the aggregate for concurrency
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    /// Gets when the aggregate was created
    /// </summary>
    public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;

    /// <summary>
    /// Gets when the aggregate was last modified
    /// </summary>
    public DateTime? LastModifiedAt { get; private set; }

    /// <summary>
    /// Marks the aggregate as modified
    /// </summary>
    protected void MarkAsModified()
    {
        LastModifiedAt = DateTime.UtcNow;
        Version++;
    }
}
