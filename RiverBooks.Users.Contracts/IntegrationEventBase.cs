using MediatR;

namespace RiverBooks.Users.Contracts;

public record IntegrationEventBase : INotification
{
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}