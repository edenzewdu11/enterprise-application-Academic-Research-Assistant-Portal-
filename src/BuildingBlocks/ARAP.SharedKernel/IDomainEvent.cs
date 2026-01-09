namespace ARAP.SharedKernel;


public interface IDomainEvent
{
   
    // Unique identifier for this event instance
   
    Guid EventId { get; }

    
    DateTime OccurredOn { get; }
}
