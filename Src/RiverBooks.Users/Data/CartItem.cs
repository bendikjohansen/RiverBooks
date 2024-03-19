using Ardalis.GuardClauses;

namespace RiverBooks.Users.Data;

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
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public void UpdateQuantity(int newQuantity)
    {
        Quantity = newQuantity;
    }

    internal void UpdateDescription(string description)
    {
        Description = description;
    }

    internal void UpdateUnitPrice(decimal unitPrice)
    {
        UnitPrice = unitPrice;
    }
}