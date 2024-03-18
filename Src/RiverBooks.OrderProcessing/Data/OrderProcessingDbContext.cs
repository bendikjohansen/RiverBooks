using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiverBooks.OrderProcessing.Data;

internal class OrderProcessingDbContext(DbContextOptions<OrderProcessingDbContext> options) : DbContext(options)
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
}

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.ComplexProperty(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.Street1).HasMaxLength(Constants.StreetMaxLength);
            address.Property(a => a.Street2).HasMaxLength(Constants.StreetMaxLength);
            address.Property(a => a.City).HasMaxLength(Constants.CityMaxLength);
            address.Property(a => a.State).HasMaxLength(Constants.StateMaxLength);
            address.Property(a => a.PostalCode).HasMaxLength(Constants.PostalCodeMaxLength);
            address.Property(a => a.Country).HasMaxLength(Constants.CountryMaxLength);
        });
        builder.ComplexProperty(o => o.BillingAddress, address =>
        {
            address.Property(a => a.Street1).HasMaxLength(Constants.StreetMaxLength);
            address.Property(a => a.Street2).HasMaxLength(Constants.StreetMaxLength);
            address.Property(a => a.City).HasMaxLength(Constants.CityMaxLength);
            address.Property(a => a.State).HasMaxLength(Constants.StateMaxLength);
            address.Property(a => a.PostalCode).HasMaxLength(Constants.PostalCodeMaxLength);
            address.Property(a => a.Country).HasMaxLength(Constants.CountryMaxLength);
        });
    }
}

internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Description).HasMaxLength(Constants.DescriptionMaxLength).IsRequired();
    }
}

internal static class Constants
{
    internal const int StreetMaxLength = 50;
    internal const int CityMaxLength = 50;
    internal const int StateMaxLength = 50;
    internal const int PostalCodeMaxLength = 20;
    internal const int CountryMaxLength = 50;
    internal const int DescriptionMaxLength = 100;
}