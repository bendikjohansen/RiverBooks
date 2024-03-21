using Ardalis.Result;

using MediatR;

using RiverBooks.EmailSending.Contracts;

namespace RiverBooks.EmailSending.Integrations;

internal class QueueEmailInOutboxSendEmailCommandHandler(IOutboxService outboxService) : IRequestHandler<SendEmailCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(SendEmailCommand request, CancellationToken ct)
    {
        var newEntity = new EmailOutboxEntity(
            request.To,
            request.From,
            request.Subject,
            request.Body);

        await outboxService.QueueEmailForSending(newEntity);

        return newEntity.Id;
    }
}