using MediatR;

using RiverBooks.OrderProcessing.Contracts;
using RiverBooks.OrderProcessing.Domain;

namespace RiverBooks.OrderProcessing.Integrations;

internal class PublishCreatedOrderIntegrationEventHandler(IPublisher mediator) : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken ct)
    {
        var dto = new OrderDetailDto
        {
            DateCreated = notification.Order.DateCreated,
            OrderId = notification.Order.Id,
            UserId = notification.Order.UserId,
            OrderItems = notification.Order.OrderItems
                .Select(oi => new OrderItemDetails(oi.BookId, oi.Quantity, oi.UnitPrice, oi.Description))
                .ToList()
        };

        var integrationEvent = new OrderCreatedIntegrationEvent(dto);

        await mediator.Publish(integrationEvent, ct);
    }
}