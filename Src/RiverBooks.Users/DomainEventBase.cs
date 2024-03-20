using MediatR;

namespace RiverBooks.Users;

internal record DomainEventBase : INotification
{
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}