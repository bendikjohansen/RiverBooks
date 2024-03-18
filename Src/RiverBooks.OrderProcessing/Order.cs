namespace RiverBooks.OrderProcessing;

internal record Order
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; private set; } = default!;
    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.UtcNow;

    private void AddOrderItems(params OrderItem[] items) => _orderItems.AddRange(items);

    internal static class Factory
    {
        public static Order Create(Guid userId, Address shippingAddress, Address billingAddress,
            IEnumerable<OrderItem> orderItems)
        {
            var order = new Order
            {
                UserId = userId, ShippingAddress = shippingAddress, BillingAddress = billingAddress
            };
            order.AddOrderItems(orderItems.ToArray());
            return order;
        }
    }
}