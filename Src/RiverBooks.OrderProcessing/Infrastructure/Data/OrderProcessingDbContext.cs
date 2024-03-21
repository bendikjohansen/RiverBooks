using System.Reflection;

using Microsoft.EntityFrameworkCore;

using RiverBooks.OrderProcessing.Domain;
using RiverBooks.SharedKernel;

namespace RiverBooks.OrderProcessing.Infrastructure.Data;

internal class OrderProcessingDbContext(DbContextOptions<OrderProcessingDbContext> options, IDomainEventDispatcher? dispatcher) : DbContext(options)
{
    internal DbSet<Order> Orders { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("OrderProcessing");
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