namespace ARAP.SharedKernel;

/// <summary>
/// Interface for domain events in the system
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Unique identifier for this event instance
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// Timestamp when the event occurred
    /// </summary>
    DateTime OccurredOn { get; }
}
