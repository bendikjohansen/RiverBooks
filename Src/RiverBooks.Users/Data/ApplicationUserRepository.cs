using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users.Data;

internal interface IApplicationUserRepository
{
    Task<ApplicationUser> GetUserWithCartByEmailAsync(string email);
    Task SaveChangesAsync();
    Task<ApplicationUser> GetUserWithAddressesByEmail(string email);
}

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

    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}