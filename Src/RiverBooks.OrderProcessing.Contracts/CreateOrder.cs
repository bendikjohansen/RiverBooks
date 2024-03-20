using Ardalis.Result;

using MediatR;

namespace RiverBooks.OrderProcessing.Contracts;

public record CreateOrderCommand(Guid UserId,
    Guid ShippingAddressId,
    Guid BillingAddressId,
    List<OrderItemDetails> OrderItems) : IRequest<Result<OrderDetailsResponse>>;

public record OrderItemDetails(Guid BookId, int Quantity, decimal UnitPrice, string Description);

public record OrderDetailsResponse(Guid OrderId);