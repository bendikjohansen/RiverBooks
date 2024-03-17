using System.Security.Claims;

using Ardalis.Result;

using FastEndpoints;

using MediatR;

using RiverBooks.Users.UseCases;

namespace RiverBooks.Users.CartEndpoints;

public record CartResponse(List<CartItemDto> CartItems);

public class ListCartItems(ISender mediator) : EndpointWithoutRequest<CartResponse>
{
    private const string EmailAddress = nameof(EmailAddress);

    public override async Task HandleAsync(CancellationToken ct)
    {
        var emailAddress = User.FindFirstValue(EmailAddress)!;

        var query = new ListCartItemsQuery(emailAddress);

        var result = await mediator.Send(query, ct);

        if (result.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(ct);
        }
        else
        {
            var response = new CartResponse(result.Value);
            await SendAsync(response, cancellation: ct);
        }
    }

    public override void Configure()
    {
        Get("/cart");
        Claims(EmailAddress);
    }
}