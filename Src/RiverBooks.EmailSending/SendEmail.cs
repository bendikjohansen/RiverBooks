using Ardalis.Result;

using MediatR;

using RiverBooks.EmailSending.Contracts;


namespace RiverBooks.EmailSending;

internal class SendEmailCommandHandler(ISendEmail emailSender) : IRequestHandler<SendEmailCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(SendEmailCommand request, CancellationToken ct)
    {
        await emailSender.SendEmailAsync(request.To,
            request.From,
            request.Subject,
            request.Body);

        return Guid.Empty;
    }

}