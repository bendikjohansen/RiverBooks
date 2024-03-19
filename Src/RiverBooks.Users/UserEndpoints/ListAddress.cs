using System.Security.Claims;

using Ardalis.Result;

using FastEndpoints;

using MediatR;

using RiverBooks.Users.UseCases.User;

namespace RiverBooks.Users.UserEndpoints;

internal sealed class ListAddress(ISender mediator) : EndpointWithoutRequest<AddressListResponse>
{
    private const string EmailAddress = "EmailAddress";

    public override void Configure()
    {
        Get("/user/address");
        Claims(EmailAddress);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var emailAddress = User.FindFirstValue(EmailAddress)!;

        var result = await mediator.Send(new ListAddressesForEmailQuery(emailAddress), ct);

        if (result.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(ct);
        }
        else
        {
            var response = new AddressListResponse(result.Value);
            await SendAsync(response, cancellation: ct);
        }
    }
}