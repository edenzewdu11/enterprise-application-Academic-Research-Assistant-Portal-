namespace ARAP.SharedKernel;


public abstract record DomainEvent : IDomainEvent
{
    // <inheritdoc />
    public Guid EventId { get; init; } = Guid.NewGuid();

    // <inheritdoc />
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
