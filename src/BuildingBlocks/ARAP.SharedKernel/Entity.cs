// =================================================================================================
// ARAP Shared Kernel - Entity Base Class
// =================================================================================================
// 
// This file contains the base Entity class that serves as the foundation for all
// domain entities in the ARAP system. It provides common functionality including:
// - Identity management with strongly-typed IDs
// - Domain events support for event-driven architecture
// - Equality comparison based on entity identity
// - Domain event management (raise, get, clear)
// 
// The Entity class follows Domain-Driven Design (DDD) principles and is part of the
// shared kernel that can be used across all modules in the system.
// =================================================================================================

namespace ARAP.SharedKernel;

/// <summary>
/// Base class for all domain entities in the ARAP system.
/// Provides identity management, domain events support, and equality comparison.
/// </summary>
/// <typeparam name="TId">The type of the entity identifier. Must be non-nullable.</typeparam>
/// <remarks>
/// This base class implements the Entity pattern from Domain-Driven Design (DDD).
/// All entities inherit from this class to ensure consistent identity management
/// and domain event handling across the entire system.
/// 
/// Key features:
/// - Strongly-typed entity IDs for type safety
/// - Domain events support for loose coupling and event-driven architecture
/// - Equality comparison based on entity identity, not reference equality
/// - Immutable ID property to prevent accidental identity changes
/// </remarks>
public abstract class Entity<TId> : IEquatable<Entity<TId>>, IHasDomainEvents
    where TId : notnull
{
    // Private field to store domain events
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    /// <remarks>
    /// The ID is protected init, meaning it can only be set during object initialization
    /// by derived classes. This prevents accidental identity changes after creation.
    /// </remarks>
    public TId Id { get; protected init; }

    /// <summary>
    /// Gets a read-only collection of domain events raised by this entity.
    /// </summary>
    /// <remarks>
    /// Returns a read-only view of the domain events to prevent external modification.
    /// Events are stored internally and can be cleared after processing.
    /// </remarks>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the Entity class with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the entity.</param>
    /// <remarks>
    /// This constructor should be used when creating entities with a known identifier,
    /// such as when loading from a database or reconstructing from events.
    /// </remarks>
    protected Entity(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the Entity class for ORM frameworks.
    /// </summary>
    /// <remarks>
    /// Parameterless constructor is required by some ORM frameworks (like Entity Framework)
    /// for proxy generation and materialization. The CS8618 warning is disabled because
    /// the ID will be set by the ORM framework during entity materialization.
    /// This constructor should not be used directly in application code.
    /// </remarks>
#pragma warning disable CS8618
    protected Entity()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Raises a domain event for this entity.
    /// </summary>
    /// <param name="domainEvent">The domain event to raise.</param>
    /// <remarks>
    /// Domain events represent something that happened in the domain that domain experts
    /// care about. They are used to achieve loose coupling between different parts of the system
    /// and enable event-driven architecture patterns.
    /// 
    /// Events are stored internally and should be processed by infrastructure components
    /// like domain event handlers or outbox pattern implementations.
    /// </remarks>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Gets all domain events raised by this entity as a read-only list.
    /// </summary>
    /// <returns>A read-only list of domain events.</returns>
    /// <remarks>
    /// This method provides access to all pending domain events that need to be processed.
    /// The returned list is read-only to prevent external modification.
    /// Use ClearDomainEvents() to remove events after processing.
    /// </remarks>
    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    /// <summary>
    /// Clears all domain events from this entity.
    /// </summary>
    /// <remarks>
    /// This method should be called after domain events have been processed
    /// to prevent duplicate processing. Typically called by infrastructure
    /// components like domain event dispatchers or outbox pattern implementations.
    /// </remarks>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    #region Equality Implementation

    /// <summary>
    /// Determines whether this entity is equal to another entity of the same type.
    /// </summary>
    /// <param name="other">The entity to compare with this instance.</param>
    /// <returns>true if the specified entity is equal to this entity; otherwise, false.</returns>
    /// <remarks>
    /// Two entities are considered equal if:
    /// 1. They are not null
    /// 2. They are not the same reference (but could be)
    /// 3. They are of the same concrete type
    /// 4. They have the same ID value
    /// 
    /// This implements value-based equality for entities based on their identity,
    /// which is the standard approach in Domain-Driven Design.
    /// </remarks>
    public bool Equals(Entity<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <summary>
    /// Determines whether this entity is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>true if the specified object is equal to this entity; otherwise, false.</returns>
    /// <remarks>
    /// This method provides the standard Equals implementation that works with
    /// the Entity type hierarchy. It delegates to the typed Equals method.
    /// </remarks>
    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Equals(entity);
    }

    /// <summary>
    /// Returns the hash code for this entity.
    /// </summary>
    /// <returns>A hash code for the current entity.</returns>
    /// <remarks>
    /// The hash code is based on the entity's ID value, ensuring that
    /// entities with the same ID will have the same hash code.
    /// This is consistent with the Equals implementation.
    /// </remarks>
    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Id);
    }

    /// <summary>
    /// Determines whether two entities are equal.
    /// </summary>
    /// <param name="left">The left entity to compare.</param>
    /// <param name="right">The right entity to compare.</param>
    /// <returns>true if the entities are equal; otherwise, false.</returns>
    /// <remarks>
    /// This operator provides a convenient way to compare entities using == syntax.
    /// It handles null references correctly and delegates to the Equals method.
    /// </remarks>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two entities are not equal.
    /// </summary>
    /// <param name="left">The left entity to compare.</param>
    /// <param name="right">The right entity to compare.</param>
    /// <returns>true if the entities are not equal; otherwise, false.</returns>
    /// <remarks>
    /// This operator provides a convenient way to compare entities using != syntax.
    /// It returns the negation of the equality comparison.
    /// </remarks>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !Equals(left, right);
    }

    #endregion
}
