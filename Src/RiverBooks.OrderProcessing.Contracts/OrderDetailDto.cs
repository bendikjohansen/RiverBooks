namespace RiverBooks.OrderProcessing.Contracts;

public record OrderDetailDto
{
    public Guid OrderId { get; init; }
    public Guid UserId { get; init; }
    public DateTimeOffset DateCreated { get; init; }
    public List<OrderItemDetails> OrderItems { get; init; } = [];
}