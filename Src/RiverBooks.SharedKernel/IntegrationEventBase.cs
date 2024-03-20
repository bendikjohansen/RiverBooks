using MediatR;

namespace RiverBooks.SharedKernel;

public record IntegrationEventBase : INotification
{
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}