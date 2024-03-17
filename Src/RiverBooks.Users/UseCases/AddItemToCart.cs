using Ardalis.Result;

using MediatR;

namespace RiverBooks.Users.UseCases;

public record AddItemToCartCommand(Guid BookId, int Quantity, string EmailAddress) : IRequest<Result>;

internal class AddItemToCartHandler(IApplicationUserRepository userRepository) : IRequestHandler<AddItemToCartCommand, Result>
{
    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserWithCartByEmailAsync(request.EmailAddress);

        if (user is null)
        {
            return Result.Unauthorized();
        }

        var cartItem = new CartItem(request.BookId, nameof(CartItem.Description), request.Quantity, 1.00m);
        user.AddItemToCart(cartItem);
        await userRepository.SaveChangesAsync();

        return Result.Success();
    }
}