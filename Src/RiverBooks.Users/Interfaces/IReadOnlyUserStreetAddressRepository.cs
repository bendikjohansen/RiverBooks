using RiverBooks.Users.Domain;

namespace RiverBooks.Users.Interfaces;

internal interface IReadOnlyUserStreetAddressRepository
{
    Task<UserStreetAddress?> GetById(Guid userStreetAddressId);
}