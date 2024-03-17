using Ardalis.GuardClauses;

using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    private readonly List<CartItem> _cartItems = new();

    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    public void AddItemToCart(CartItem item)
    {
        Guard.Against.Null(item);

        var existingBook = CartItems.SingleOrDefault(c => c.BookId == item.BookId);
        if (existingBook is not null)
        {
            existingBook.UpdateQuantity(existingBook.Quantity + item.Quantity);
            // TODO: What if other details have changed?
            return;
        }

        _cartItems.Add(item);
    }
}

public record CartItem
{
    public CartItem(Guid bookId, string description, int quantity, decimal unitPrice)
    {
        BookId = Guard.Against.Default(bookId);
        Description = Guard.Against.NullOrEmpty(description);
        Quantity = Guard.Against.Negative(quantity);
        UnitPrice = Guard.Against.Negative(unitPrice);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CartItem() { } // EF
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid BookId { get; init; }
    public string Description { get; init; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; init; }

    public void UpdateQuantity(int newQuantity)
    {
        Quantity = newQuantity;
    }
}