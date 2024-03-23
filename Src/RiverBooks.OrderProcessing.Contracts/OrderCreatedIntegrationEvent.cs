using MediatR;

namespace RiverBooks.OrderProcessing.Contracts;

public record OrderCreatedIntegrationEvent(OrderDetailDto OrderDetails) : INotification
{
    public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.Now;
}