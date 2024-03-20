using Ardalis.Result;

using MediatR;

using RiverBooks.Users.Interfaces;
using RiverBooks.Users.UserEndpoints;

namespace RiverBooks.Users.UseCases.User;

internal record ListAddressesForEmailQuery(string Email) : IRequest<Result<List<UserAddressDto>>>;

internal class ListAddressesForEmailHandler(IApplicationUserRepository repository) : IRequestHandler<ListAddressesForEmailQuery, Result<List<UserAddressDto>>>
{
    public async Task<Result<List<UserAddressDto>>> Handle(ListAddressesForEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserWithAddressesByEmail(request.Email);

        if (user == null)
        {
            return Result.Unauthorized();
        }

        var userAddresses = user.Addresses.Select(address => new UserAddressDto(address.Id,
            address.StreetAddress.Street1,
            address.StreetAddress.Street2,
            address.StreetAddress.City,
            address.StreetAddress.State,
            address.StreetAddress.PostalCode,
            address.StreetAddress.Country))
            .ToList();

        return Result.Success(userAddresses);
    }
}