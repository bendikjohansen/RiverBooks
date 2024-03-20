using Ardalis.Result;

using FluentValidation;

using MediatR;

using RiverBooks.Books.Contracts;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.UseCases.Cart;

public record AddItemToCartCommand(Guid BookId, int Quantity, string EmailAddress) : IRequest<Result>;

internal class AddItemToCartHandler(IApplicationUserRepository userRepository, ISender mediator) : IRequestHandler<AddItemToCartCommand, Result>
{
    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserWithCartByEmailAsync(request.EmailAddress);

        if (user is null)
        {
            return Result.Unauthorized();
        }

        var query = new BookDetailsQuery(request.BookId);
        var result = await mediator.Send(query, cancellationToken);
        if (result.Status == ResultStatus.NotFound)
        {
            return Result.NotFound();
        }

        var bookDetails = result.Value;
        var description = $"{bookDetails.Title} by {bookDetails.Author}";

        var cartItem = new CartItem(request.BookId, description, request.Quantity, bookDetails.Price);
        user.AddItemToCart(cartItem);
        await userRepository.SaveChangesAsync();

        return Result.Success();
    }
}

public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(x => x.EmailAddress).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.BookId).NotEmpty();
    }
}