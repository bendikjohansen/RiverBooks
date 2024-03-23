using RiverBooks.SharedKernel;

namespace RiverBooks.OrderProcessing.Domain;

internal record OrderCreatedEvent(Order Order) : DomainEventBase;