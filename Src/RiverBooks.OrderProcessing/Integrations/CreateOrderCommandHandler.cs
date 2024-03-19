using Ardalis.Result;

using MediatR;

using RiverBooks.OrderProcessing.Contracts;
using RiverBooks.OrderProcessing.Data;

using Serilog;

namespace RiverBooks.OrderProcessing.Integrations;

internal class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IOrderAddressCache addressCache,
    ILogger logger) : IRequestHandler<CreateOrderCommand, Result<OrderDetailsResponse>>
{
    public async Task<Result<OrderDetailsResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var items = request.OrderItems.Select(oi =>
            new OrderItem(oi.BookId, oi.Description, oi.Quantity, oi.UnitPrice));

        var shippingAddress = await addressCache.GetAddressByIdAsync(request.ShippingAddressId);
        var billingAddress = await addressCache.GetAddressByIdAsync(request.BillingAddressId);
        var order = Order.Factory.Create(request.UserId, shippingAddress.Value.Address, billingAddress.Value.Address, items);

        await orderRepository.AddAsync(order);
        await orderRepository.SaveChangesAsync();

        logger.Information("Order created with ID {orderId}", order.Id);
        return new OrderDetailsResponse(order.Id);
    }
}