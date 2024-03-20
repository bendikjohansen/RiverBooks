using System.Reflection;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using RiverBooks.SharedKernel;
using RiverBooks.Users.Domain;

namespace RiverBooks.Users.Infrastructure.Data;

internal class UsersDbContext(DbContextOptions<UsersDbContext> options, IDomainEventDispatcher? dispatcher) : IdentityDbContext(options)
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = default!;
    public DbSet<UserStreetAddress> UserStreetAddresses { get; set; } = default!;

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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        if (dispatcher == null)
        {
            return result;
        }

        var entitiesWithEvents = (ChangeTracker.Entries<IHaveDomainEvents>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any()))
            .ToArray();
        await dispatcher.DispatchAndClearEventsAsync(entitiesWithEvents);
        return result;
    }
}