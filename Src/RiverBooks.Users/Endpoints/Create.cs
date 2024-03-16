using System.Net;

using FastEndpoints;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ProblemDetails = FastEndpoints.ProblemDetails;

namespace RiverBooks.Users.Endpoints;

public record CreateUserRequest(string Email, string Password);

internal class Create(UserManager<ApplicationUser> userManager) : Endpoint<CreateUserRequest>
{
    public override void Configure()
    {
        Post("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var user = new ApplicationUser { Email = req.Email, UserName = req.Email };

        var result = await userManager.CreateAsync(user, req.Password);
        if (!result.Succeeded)
        {
            var problems = new ProblemDetails
            {
                Errors = result.Errors.Select(x =>
                    new ProblemDetails.Error { Code = x.Code, Reason = x.Description, Name = x.Code}),
                Detail = "Could not register user."
            };
            await SendAsync(problems, 400, ct);
            return;
        }

        await SendOkAsync(ct);
    }
}