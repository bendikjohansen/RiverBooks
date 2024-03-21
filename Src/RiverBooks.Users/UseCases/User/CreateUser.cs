using Ardalis.Result;

using MediatR;

using Microsoft.AspNetCore.Identity;

using RiverBooks.EmailSending.Contracts;
using RiverBooks.Users.Domain;

namespace RiverBooks.Users.UseCases.User;

internal record CreateUserCommand(string Email, string Password) : IRequest<Result>;

internal class CreateUserCommandHandler(UserManager<ApplicationUser> userManager, IMediator mediator) : IRequestHandler<CreateUserCommand, Result>
{
    public async Task<Result> Handle(CreateUserCommand command, CancellationToken ct)
    {
        var user = new ApplicationUser { Email = command.Email, UserName = command.Email };

        var result = await userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
        {
            return Result.Error(result.Errors.Select(error => error.Description).ToArray());
        }

        // send welcome email
        var emailCommand = new SendEmailCommand(user.Email,
            "donotreply@test.com",
            "Welcome to RiverBooks!",
            "Thank you for registering!");

        _ = await mediator.Send(emailCommand, ct);

        return Result.Success();

    }
}