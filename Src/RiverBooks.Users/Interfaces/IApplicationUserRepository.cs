using RiverBooks.Users.Domain;

namespace RiverBooks.Users.Interfaces;

internal interface IApplicationUserRepository
{
    Task<ApplicationUser> GetUserWithCartByEmailAsync(string email);
    Task SaveChangesAsync();
    Task<ApplicationUser> GetUserWithAddressesByEmail(string email);
}