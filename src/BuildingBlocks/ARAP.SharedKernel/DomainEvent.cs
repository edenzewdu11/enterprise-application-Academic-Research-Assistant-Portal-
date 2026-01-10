namespace ARAP.SharedKernel;

/// <summary>
/// Base class for all domain events in the system
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    /// <summary>
    /// Unique identifier for the event
    /// </summary>
    public Guid EventId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Timestamp when the event occurred
    /// </summary>
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
