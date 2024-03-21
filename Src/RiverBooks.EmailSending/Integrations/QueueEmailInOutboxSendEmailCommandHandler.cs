using Ardalis.Result;

using MediatR;

using RiverBooks.EmailSending.Contracts;

namespace RiverBooks.EmailSending.Integrations;

internal class QueueEmailInOutboxSendEmailCommandHandler(
    IQueueEmailsForSendingService queueEmailsForSendingService) : IRequestHandler<SendEmailCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(SendEmailCommand request, CancellationToken ct)
    {
        var newEntity = new EmailOutboxEntity(
            request.To,
            request.From,
            request.Subject,
            request.Body);

        await queueEmailsForSendingService.QueueEmailForSending(newEntity);

        return newEntity.Id;
    }
}