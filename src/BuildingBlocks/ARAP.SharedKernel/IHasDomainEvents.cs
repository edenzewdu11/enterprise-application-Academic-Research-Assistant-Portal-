namespace ARAP.SharedKernel;


public interface IHasDomainEvents
{
    
    IReadOnlyList<IDomainEvent> GetDomainEvents();

        void ClearDomainEvents();
}
