using Ardalis.Result;

using MediatR;


namespace RiverBooks.EmailSending;

public record SendEmailCommand(string To, string From, string Subject, string Body) : IRequest<Result<Guid>>;

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