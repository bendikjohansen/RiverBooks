using System.ComponentModel.DataAnnotations.Schema;

using Ardalis.GuardClauses;

using Microsoft.AspNetCore.Identity;

using RiverBooks.SharedKernel;

namespace RiverBooks.Users.Domain;

internal class ApplicationUser : IdentityUser, IHaveDomainEvents
{
    public string FullName { get; set; } = string.Empty;

    private readonly List<CartItem> _cartItems = [];
    private readonly List<UserStreetAddress> _addresses = [];
    private readonly List<DomainEventBase> _domainEvents = [];

    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();
    public IReadOnlyCollection<UserStreetAddress> Addresses => _addresses.AsReadOnly();
    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    public void AddItemToCart(CartItem item)
    {
        Guard.Against.Null(item);

        var existingBook = CartItems.SingleOrDefault(c => c.BookId == item.BookId);
        if (existingBook is not null)
        {
            existingBook.UpdateQuantity(existingBook.Quantity + item.Quantity);
            existingBook.UpdateDescription(item.Description);
            existingBook.UpdateUnitPrice(item.UnitPrice);
            return;
        }

        _cartItems.Add(item);
    }

    public void ClearCart() => _cartItems.Clear();
    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);

    public UserStreetAddress AddAddress(Address address)
    {
        Guard.Against.Null(address);

        var existingAddress = _addresses.SingleOrDefault(a => a.StreetAddress == address);
        if (existingAddress is not null)
        {
            return existingAddress;
        }

        var newAddress = new UserStreetAddress(Id, address);
        _addresses.Add(newAddress);

        RegisterDomainEvent(new AddressAddedEvent(newAddress));

        return newAddress;
    }
}