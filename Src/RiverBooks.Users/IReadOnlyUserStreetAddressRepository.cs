using RiverBooks.Users.Data;

namespace RiverBooks.Users;

internal interface IReadOnlyUserStreetAddressRepository
{
    Task<UserStreetAddress?> GetById(Guid userStreetAddressId);
}