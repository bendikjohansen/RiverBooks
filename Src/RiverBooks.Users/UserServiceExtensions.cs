using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace RiverBooks.Users;

public static class UserServiceExtensions
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
        mediatrAssemblies.Add(typeof(UserServiceExtensions).Assembly);
        services.AddScoped<IApplicationUserRepository, EfApplicationUserRepository>();

        logger.Information("{Module} module services registered", "Users");

        return services;
    }
}