namespace ARAP.SharedKernel;


public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    protected AggregateRoot(TId id) : base(id)
    {
    }

    // Required for EF Core
    protected AggregateRoot() : base()
    {
    }

    
    public int Version { get; private set; }

    
    public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;

    
    public DateTime? LastModifiedAt { get; private set; }

    
    protected void MarkAsModified()
    {
        LastModifiedAt = DateTime.UtcNow;
        Version++;
    }
}
