namespace ARAP.SharedKernel;

/// <summary>
/// Interface for entities that can raise domain events
/// </summary>
public interface IHasDomainEvents
{
    /// <summary>
    /// Gets all domain events raised by the entity
    /// </summary>
    IReadOnlyList<IDomainEvent> GetDomainEvents();

    /// <summary>
    /// Clears all domain events from the entity
    /// </summary>
    void ClearDomainEvents();
}
