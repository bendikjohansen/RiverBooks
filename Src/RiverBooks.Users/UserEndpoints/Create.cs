using Ardalis.Result.AspNetCore;

using FastEndpoints;

using MediatR;

using RiverBooks.Users.UseCases.User;

namespace RiverBooks.Users.UserEndpoints;

public record CreateUserRequest(string Email, string Password);

internal class Create(IMediator mediator) : Endpoint<CreateUserRequest>
{
    public override void Configure()
    {
        Post("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var command = new CreateUserCommand(req.Email, req.Password);

        var result = await mediator.Send(command, ct);

        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        await SendOkAsync(ct);
    }
}