namespace RiverBooks.SharedKernel;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEventsAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents);
}