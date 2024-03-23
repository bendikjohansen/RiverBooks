using Ardalis.Result;

using MongoDB.Driver;

namespace RiverBooks.EmailSending.EmailBackgroundService;

internal interface IGetEmailsFromOutboxService
{
    Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity(CancellationToken ct);
}

internal class DefaultGetEmailsFromOutboxService(IMongoCollection<EmailOutboxEntity> emailCollection) : IGetEmailsFromOutboxService
{
    public async Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity(CancellationToken ct = default)
    {
        var filter = Builders<EmailOutboxEntity>.Filter.Eq(entity => entity.ProcessedAt, null);
        var unsentEmailEntity = await emailCollection.Find(filter).FirstOrDefaultAsync(cancellationToken: ct);

        if (unsentEmailEntity is null)
        {
            return Result.NotFound();
        }

        return unsentEmailEntity;
    }
}