using FastEndpoints;
using FastEndpoints.Security;

using Microsoft.AspNetCore.Identity;

using Serilog;

namespace RiverBooks.Users.UserEndpoints;

public record UserLoginRequest(string Email, string Password);

public record UserLoginResponse(string Token);

internal class Login(UserManager<ApplicationUser> userManager, ILogger logger) : Endpoint<UserLoginRequest, UserLoginResponse>
{
    public override void Configure()
    {
        Post("/users/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UserLoginRequest req, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(req.Email);
        if (user == null)
        {
            logger.Information("User with email {email} was not found", req.Email);
            await SendUnauthorizedAsync(ct);
            return;
        }

        var loginSuccessful = await userManager.CheckPasswordAsync(user, req.Password);
        if (!loginSuccessful)
        {
            logger.Information("Wrong password for user {email}", user.Email);
            await SendUnauthorizedAsync(ct);
            return;
        }

        var jwtSecret = Config["Auth:JwtSecret"]!;
        var token = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = jwtSecret;
            o.User["EmailAddress"] = user.Email!;
        });
        await SendAsync(new UserLoginResponse(token), cancellation: ct);
    }
}