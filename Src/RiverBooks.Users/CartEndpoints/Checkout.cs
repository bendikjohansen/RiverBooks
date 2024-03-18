using System.Security.Claims;

using Ardalis.Result;

using FastEndpoints;

using MediatR;

using RiverBooks.Users.UseCases;

namespace RiverBooks.Users.CartEndpoints;

internal class Checkout(IMediator mediator) : Endpoint<CheckoutRequest, CheckoutResponse>
{
    private const string EmailAddress = "EmailAddress";
    public override void Configure()
    {
        Post("/cart/checkout");
        Claims(EmailAddress);
    }

    public override async Task HandleAsync(CheckoutRequest req, CancellationToken ct)
    {
        var emailAddress = User.FindFirstValue(EmailAddress)!;

        var command = new CheckoutCartCommand(emailAddress, req.ShippingAddressId, req.BillingAddressId);

        var result = await mediator.Send(command, ct);

        if (result.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(ct);
        }
        else
        {
            await SendAsync(new CheckoutResponse(result.Value));
        }
    }
}

internal record CheckoutRequest(Guid ShippingAddressId, Guid BillingAddressId);
internal record CheckoutResponse(Guid OrderId);