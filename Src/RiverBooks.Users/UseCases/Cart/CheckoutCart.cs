using Ardalis.Result;

using MediatR;

using RiverBooks.OrderProcessing.Contracts;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.UseCases.Cart;

public record CheckoutCartCommand(string EmailAddress, Guid ShippingAddressId, Guid BillingAddressId) : IRequest<Result<Guid>>;

internal class CheckoutCartHandler(IApplicationUserRepository userRepository, IMediator mediator)
    : IRequestHandler<CheckoutCartCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserWithCartByEmailAsync(request.EmailAddress);

        if (user is null)
        {
            return Result.Unauthorized();
        }

        var items = user.CartItems.Select(item =>
            new OrderItemDetails(item.BookId,
                item.Quantity,
                item.UnitPrice,
                item.Description))
            .ToList();

        var createOrderCommand = new CreateOrderCommand(Guid.Parse(user.Id),
            request.ShippingAddressId,
            request.BillingAddressId,
            items);

        // TODO: Consider replacing with a message-based approach for performance reasons
        var result = await mediator.Send(createOrderCommand, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.Map(x => x.OrderId);
        }

        user.ClearCart();
        await userRepository.SaveChangesAsync();

        return Result.Success(result.Value.OrderId);
    }
}