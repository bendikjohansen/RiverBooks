using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RiverBooks.OrderProcessing.Domain;

namespace RiverBooks.OrderProcessing.Infrastructure.Data;

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