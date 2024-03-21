using System.ComponentModel.DataAnnotations.Schema;

using RiverBooks.SharedKernel;

namespace RiverBooks.OrderProcessing.Domain;

internal record Order : IHaveDomainEvents
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; private set; } = default!;
    private readonly List<OrderItem> _orderItems = [];
    private readonly List<DomainEventBase> _domainEvents = [];
    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.UtcNow;

    private void AddOrderItems(params OrderItem[] items) => _orderItems.AddRange(items);

    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
    internal static class Factory
    {
        public static Order Create(Guid userId, Address shippingAddress, Address billingAddress,
            IEnumerable<OrderItem> orderItems)
        {
            var order = new Order
            {
                UserId = userId, ShippingAddress = shippingAddress, BillingAddress = billingAddress
            };
            var createdEvent = new OrderCreatedEvent(order);
            order.RegisterDomainEvent(createdEvent);
            order.AddOrderItems(orderItems.ToArray());

            return order;
        }
    }
}