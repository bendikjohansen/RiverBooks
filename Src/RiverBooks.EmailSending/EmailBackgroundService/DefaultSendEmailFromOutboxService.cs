using Ardalis.Result;

using MongoDB.Driver;

namespace RiverBooks.EmailSending.EmailBackgroundService;

internal interface ISendEmailFromOutboxService
{
    Task CheckForAndSendEmails(CancellationToken ct);
}

internal class DefaultSendEmailFromOutboxService(
    IGetEmailsFromOutboxService getEmailsFromOutboxService,
    ISendEmail emailSender,
    IMongoCollection<EmailOutboxEntity> emailCollection) : ISendEmailFromOutboxService
{
    public async Task CheckForAndSendEmails(CancellationToken ct = default)
    {
        var result = await getEmailsFromOutboxService.GetUnprocessedEmailEntity(ct);

        if (result.Status == ResultStatus.NotFound)
        {
            return;
        }

        var emailEntity = result.Value;
        await emailSender.SendEmailAsync(emailEntity.To, emailEntity.From, emailEntity.Subject, emailEntity.Body, ct);

        var updateFilter = Builders<EmailOutboxEntity>.Filter.Eq(x => x.Id, emailEntity.Id);
        var update = Builders<EmailOutboxEntity>.Update
            .Set(nameof(EmailOutboxEntity.ProcessedAt), DateTimeOffset.UtcNow);
        await emailCollection.UpdateOneAsync(updateFilter, update, cancellationToken: ct);
    }
}