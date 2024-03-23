using MailKit.Net.Smtp;

using Microsoft.Extensions.Logging;

using MimeKit;

namespace RiverBooks.EmailSending.EmailBackgroundService;

internal interface ISendEmail
{
    Task SendEmailAsync(string to, string from, string subject, string body, CancellationToken ct);
}

internal class MimeKitEmailSender(ILogger<MimeKitEmailSender> logger) : ISendEmail
{
    public async Task SendEmailAsync(string to, string from, string subject, string body, CancellationToken ct = default)
    {
        logger.LogInformation("Attempting to send email to {to} from {from} with subject {subject}.", to, from,
            subject);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(from, from));
        message.To.Add(new MailboxAddress(to, to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync("localhost", 25, false, ct);

        await client.SendAsync(message, ct);
        logger.LogInformation("Email sent!");

        await client.DisconnectAsync(false, ct).ConfigureAwait(false);
    }
}