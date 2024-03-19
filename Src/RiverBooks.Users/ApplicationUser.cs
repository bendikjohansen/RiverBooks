using Ardalis.GuardClauses;

using Microsoft.AspNetCore.Identity;

using RiverBooks.Users.Data;
using RiverBooks.Users.UseCases.User;

namespace RiverBooks.Users;

internal class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    private readonly List<CartItem> _cartItems = [];
    private readonly List<UserStreetAddress> _addresses = [];

    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();
    public IReadOnlyCollection<UserStreetAddress> Addresses => _addresses.AsReadOnly();

    public void AddItemToCart(CartItem item)
    {
        Guard.Against.Null(item);

        var existingBook = CartItems.SingleOrDefault(c => c.BookId == item.BookId);
        if (existingBook is not null)
        {
            existingBook.UpdateQuantity(existingBook.Quantity + item.Quantity);
            // TODO: What if other details have changed?
            existingBook.UpdateDescription(item.Description);
            existingBook.UpdateUnitPrice(item.UnitPrice);
            return;
        }

        _cartItems.Add(item);
    }

    public void ClearCart() => _cartItems.Clear();

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

        return newAddress;
    }
}