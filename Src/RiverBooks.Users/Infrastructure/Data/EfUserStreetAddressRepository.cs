using Microsoft.EntityFrameworkCore;

using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Infrastructure.Data;

internal class EfUserStreetAddressRepository(UsersDbContext dbContext) : IReadOnlyUserStreetAddressRepository
{
    public Task<UserStreetAddress?> GetById(Guid userStreetAddressId) =>
        dbContext.UserStreetAddresses.SingleOrDefaultAsync(a => a.Id == userStreetAddressId);
}