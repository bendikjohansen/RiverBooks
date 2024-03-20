using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users.Data;

internal class EfUserStreetAddressRepository(UsersDbContext dbContext) : IReadOnlyUserStreetAddressRepository
{
    public Task<UserStreetAddress?> GetById(Guid userStreetAddressId) =>
        dbContext.UserStreetAddresses.SingleOrDefaultAsync(a => a.Id == userStreetAddressId);
}