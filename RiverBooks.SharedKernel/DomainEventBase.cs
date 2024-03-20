using MediatR;

namespace RiverBooks.SharedKernel;

public record DomainEventBase : INotification
{
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}