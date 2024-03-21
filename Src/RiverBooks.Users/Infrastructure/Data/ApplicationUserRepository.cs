using Microsoft.EntityFrameworkCore;

using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Infrastructure.Data;

internal class EfApplicationUserRepository(UsersDbContext dbContext) : IApplicationUserRepository
{
    public Task<ApplicationUser> GetUserWithCartByEmailAsync(string email) =>
        dbContext.ApplicationUsers
            .Include(user => user.CartItems)
            .SingleAsync(user => user.Email == email);

    public Task<ApplicationUser> GetUserWithAddressesByEmail(string email) =>
        dbContext.ApplicationUsers
            .Include(user => user.Addresses)
            .SingleAsync(user => user.Email == email);

    public Task<ApplicationUser> GetUserByIdAsync(Guid userId) =>
        dbContext.ApplicationUsers.SingleAsync(user => user.Id == userId.ToString());


    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}