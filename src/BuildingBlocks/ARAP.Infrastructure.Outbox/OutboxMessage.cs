// =================================================================================================
// ARAP Infrastructure - Outbox Pattern Implementation
// =================================================================================================
// 
// This file contains the OutboxMessage entity which implements the Transactional Outbox Pattern.
// The outbox pattern ensures reliable event publishing by storing events in the same
// transaction as the business logic, then processing them asynchronously.
// 
// Key benefits:
// - Guarantees event publication even if the application crashes after committing
// - Prevents duplicate event publication through idempotent processing
// - Enables retry mechanisms for failed event publications
// - Supports eventual consistency in distributed systems
// =================================================================================================

namespace ARAP.Infrastructure.Outbox;

/// <summary>
/// Represents a domain event stored in the outbox for reliable publication.
/// </summary>
/// <remarks>
/// The OutboxMessage entity implements the Transactional Outbox Pattern to ensure
/// reliable event publishing in distributed systems. When a domain event is raised
/// within a business transaction, it's stored as an OutboxMessage in the same transaction.
/// A separate background process then processes these messages and publishes them
/// to the appropriate message broker or event store.
/// 
/// This approach prevents the common problem where events are lost due to:
/// - Application crashes after database commit but before event publication
/// - Network failures during event publication
/// - Message broker unavailability
/// 
/// The message includes retry logic and error handling to ensure eventual delivery.
/// </remarks>
public sealed class OutboxMessage
{
    /// <summary>
    /// Gets or sets the unique identifier for the outbox message.
    /// </summary>
    /// <remarks>
    /// Uses GUID to ensure uniqueness across all messages and prevent collisions.
    /// This identifier is used for tracking and deduplication purposes.
    /// </remarks>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the full type name of the domain event.
    /// </summary>
    /// <remarks>
    /// Stores the assembly-qualified type name to enable deserialization
    /// of the domain event during processing. This allows the outbox processor
    /// to reconstruct the original domain event object.
    /// Example: "ARAP.Modules.ResearchProposal.Events.ResearchProposalSubmitted, ARAP.Modules.ResearchProposal"
    /// </remarks>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the serialized content of the domain event.
    /// </summary>
    /// <remarks>
    /// Contains the JSON-serialized representation of the domain event.
    /// The content includes all event data needed for processing by subscribers.
    /// Serialization is typically performed using System.Text.Json or similar serializer.
    /// </remarks>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC timestamp when the domain event occurred.
    /// </summary>
    /// <remarks>
    /// Represents the original occurrence time of the domain event.
    /// This timestamp is crucial for:
    /// - Event ordering and temporal consistency
    /// - Debugging and audit trails
    /// - Time-based event processing rules
    /// - Expiration policies for old events
    /// </remarks>
    public DateTime OccurredOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the message was successfully processed.
    /// </summary>
    /// <remarks>
    /// Null when the message hasn't been processed yet.
    /// Set to the processing completion time when the event is successfully published.
    /// This field is used for:
    /// - Monitoring processing latency
    /// - Identifying stuck or failed messages
    /// - Cleanup of old processed messages
    /// </remarks>
    public DateTime? ProcessedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the error message if processing failed.
    /// </summary>
    /// <remarks>
    /// Contains the exception details when message processing fails.
    /// This information is valuable for:
    /// - Debugging processing failures
    /// - Determining retry strategies
    /// - Alerting on critical failures
    /// - Manual intervention decisions
    /// Null when processing succeeds or hasn't been attempted.
    /// </remarks>
    public string? Error { get; set; }

    /// <summary>
    /// Gets or sets the number of retry attempts for this message.
    /// </summary>
    /// <remarks>
    /// Tracks how many times the system has attempted to process this message.
    /// Used to implement retry policies and prevent infinite retry loops.
    /// Typical retry strategies:
    /// - Exponential backoff between retries
    /// - Maximum retry limits (e.g., 3-5 attempts)
    /// - Dead letter queue after max retries
    /// - Different retry strategies for different error types
    /// </remarks>
    public int RetryCount { get; set; }
}
