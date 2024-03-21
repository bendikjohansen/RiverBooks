namespace RiverBooks.EmailSending;

public record EmailOutboxEntity(string To, string From, string Subject, string Body)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTimeOffset? ProcessedAt { get; init; }
};