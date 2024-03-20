using MediatR;

namespace RiverBooks.Users;

internal interface IDomainEventDispatcher
{
    Task DispatchAndClearEventsAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents);
}

internal class MediatRDomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchAndClearEventsAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents)
    {
        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();
            foreach (var domainEvent in events)
            {
                await mediator.Publish(domainEvent).ConfigureAwait(false);
            }
        }
    }
}