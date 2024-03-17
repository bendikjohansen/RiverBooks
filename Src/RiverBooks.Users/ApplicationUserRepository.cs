using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users;

internal interface IApplicationUserRepository
{
    Task<ApplicationUser> GetUserWithCartByEmailAsync(string email);
    Task SaveChangesAsync();
}

internal class EfApplicationUserRepository(UsersDbContext dbContext) : IApplicationUserRepository
{
    public Task<ApplicationUser> GetUserWithCartByEmailAsync(string email) =>
        dbContext.ApplicationUsers
            .Include(user => user.CartItems)
            .SingleAsync(user => user.Email == email);

    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}