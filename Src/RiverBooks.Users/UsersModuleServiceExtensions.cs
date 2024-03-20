using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RiverBooks.Users.Domain;
using RiverBooks.Users.Infrastructure.Data;
using RiverBooks.Users.Interfaces;

using Serilog;

namespace RiverBooks.Users;

public static class UsersModuleServiceExtensions
{
    public static IServiceCollection AddUserModuleServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        List<Assembly> mediatrAssemblies)
    {
        var connectionString = configuration.GetConnectionString("Users");
        services.AddDbContext<UsersDbContext>(config => config.UseNpgsql(connectionString));
        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<UsersDbContext>();
        mediatrAssemblies.Add(typeof(UsersModuleServiceExtensions).Assembly);
        services.AddScoped<IApplicationUserRepository, EfApplicationUserRepository>();
        services.AddScoped<IReadOnlyUserStreetAddressRepository, EfUserStreetAddressRepository>();

        logger.Information("{Module} module services registered", "Users");

        return services;
    }
}