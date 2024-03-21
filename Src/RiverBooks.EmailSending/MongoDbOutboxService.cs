using Ardalis.Result;

using MongoDB.Driver;

namespace RiverBooks.EmailSending;

internal interface IOutboxService
{
    Task QueueEmailForSending(EmailOutboxEntity entity);
    Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity();
}

internal class MongoDbOutboxService(IMongoCollection<EmailOutboxEntity> emailCollection) : IOutboxService
{
    public async Task QueueEmailForSending(EmailOutboxEntity entity)
    {
        await emailCollection.InsertOneAsync(entity);
    }

    public async Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity()
    {
        var filter = Builders<EmailOutboxEntity>.Filter.Eq(entity => entity.ProcessedAt, null);
        var unsentEmailEntity = await emailCollection.Find(filter).FirstOrDefaultAsync();

        if (unsentEmailEntity is null)
        {
            return Result.NotFound();
        }
        else
        {
            return unsentEmailEntity;
        }
    }
}