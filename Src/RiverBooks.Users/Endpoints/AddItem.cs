using System.Security.Claims;

using Ardalis.Result;

using FastEndpoints;

using MediatR;

using RiverBooks.Users.UseCases;

namespace RiverBooks.Users.Endpoints;

public record AddCartItemRequest(Guid BookId, int Quantity);

internal class AddItem(IMediator mediator) : Endpoint<AddCartItemRequest>
{
    private const string EmailAddress = nameof(EmailAddress);

    public override void Configure()
    {
        Post("/cart");
        Claims(EmailAddress);
    }

    public override async Task HandleAsync(AddCartItemRequest req, CancellationToken ct)
    {
        var emailAddress = User.FindFirstValue(EmailAddress) ?? throw new Exception("No email address claim was found");

        var command = new AddItemToCartCommand(req.BookId, req.Quantity, emailAddress);
        var result = await mediator.Send(command, ct);

        if (result.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        await SendOkAsync(ct);
    }
}