using Ardalis.Result;

using MediatR;

using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

using Serilog;

namespace RiverBooks.Users.UseCases.User;

internal record AddAddressToUserCommand(
    string EmailAddress,
    string Street1,
    string Street2,
    string City,
    string State,
    string PostalCode,
    string Country) : IRequest<Result>;

internal class AddAddressToUserHandler(IApplicationUserRepository userRepository, ILogger logger) : IRequestHandler<AddAddressToUserCommand, Result>
{
    public async Task<Result> Handle(AddAddressToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserWithAddressesByEmail(request.EmailAddress);

        if (user is null)
        {
            return Result.Unauthorized();
        }

        var address = new Address(request.Street1, request.Street2, request.City, request.State,
            request.PostalCode, request.Country);
        var userAddress = user.AddAddress(address);
        await userRepository.SaveChangesAsync();

        logger.Information("Added address {address} to user {email} (Total: {userAddresses})",
            userAddress,
            user.Email,
            user.Addresses.Count);

        return Result.Success();
    }
}