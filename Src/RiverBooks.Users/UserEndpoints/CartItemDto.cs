namespace RiverBooks.Users.UserEndpoints;

public record CartItemDto(Guid Id, Guid BookId, string Description, int Quantity, decimal UnitPrice);