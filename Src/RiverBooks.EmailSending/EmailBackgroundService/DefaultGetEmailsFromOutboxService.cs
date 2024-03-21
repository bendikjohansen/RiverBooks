using Ardalis.Result;

using MongoDB.Driver;

namespace RiverBooks.EmailSending;

internal interface IGetEmailsFromOutboxService
{
    Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity();
}

internal class DefaultGetEmailsFromOutboxService(IMongoCollection<EmailOutboxEntity> emailCollection) : IGetEmailsFromOutboxService
{
    public async Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity()
    {
        var filter = Builders<EmailOutboxEntity>.Filter.Eq(entity => entity.ProcessedAt, null);
        var unsentEmailEntity = await emailCollection.Find(filter).FirstOrDefaultAsync();

        if (unsentEmailEntity is null)
        {
            return Result.NotFound();
        }

        return unsentEmailEntity;
    }
}