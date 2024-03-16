using System.Reflection;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        ILogger logger)
    {
        var connectionString = configuration.GetConnectionString("Users");
        services.AddDbContext<UsersDbContext>(config => config.UseNpgsql(connectionString));
        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<UsersDbContext>();

        logger.Information("{Module} module services registered", "Users");

        return services;
    }
}

public class ApplicationUser : IdentityUser
{
}

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : IdentityDbContext(options)
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Users");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }
}