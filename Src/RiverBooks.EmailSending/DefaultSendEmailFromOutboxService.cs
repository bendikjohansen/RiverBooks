using Ardalis.Result;

using MongoDB.Driver;

namespace RiverBooks.EmailSending;

internal class DefaultSendEmailFromOutboxService(
    IOutboxService outboxService,
    ISendEmail emailSender,
    IMongoCollection<EmailOutboxEntity> emailCollection) : ISendEmailFromOutboxService
{
    public async Task CheckForAndSendEmails()
    {
        var result = await outboxService.GetUnprocessedEmailEntity();

        if (result.Status == ResultStatus.NotFound)
        {
            return;
        }

        var emailEntity = result.Value;
        await emailSender.SendEmailAsync(emailEntity.To, emailEntity.From, emailEntity.Subject, emailEntity.Body);

        var updateFilter = Builders<EmailOutboxEntity>.Filter.Eq(x => x.Id, emailEntity.Id);
        var update = Builders<EmailOutboxEntity>.Update
            .Set(nameof(EmailOutboxEntity.ProcessedAt), DateTimeOffset.UtcNow);
        await emailCollection.UpdateOneAsync(updateFilter, update);
    }
}