using MongoDB.Driver;

namespace RiverBooks.EmailSending;

internal interface IQueueEmailsForSendingService
{
    Task QueueEmailForSending(EmailOutboxEntity entity);
}

internal class DefaultQueueEmailsForSendingService(IMongoCollection<EmailOutboxEntity> emailCollection) : IQueueEmailsForSendingService
{
    public async Task QueueEmailForSending(EmailOutboxEntity entity)
    {
        await emailCollection.InsertOneAsync(entity);
    }
}