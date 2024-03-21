using MailKit.Net.Smtp;

using Microsoft.Extensions.Logging;

using MimeKit;

namespace RiverBooks.EmailSending;

internal interface ISendEmail
{
    Task SendEmailAsync(string to, string from, string subject, string body);
}

internal class MimeKitEmailSender(ILogger<MimeKitEmailSender> logger) : ISendEmail
{
    public async Task SendEmailAsync(string to, string from, string subject, string body)
    {
        logger.LogInformation("Attempting to send email to {to} from {from} with subject {subject}.", to, from,
            subject);

        using var client = new SmtpClient();
        await client.ConnectAsync("localhost", 25, false);
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(from, from));
        message.To.Add(new MailboxAddress(to, to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        await client.SendAsync(message);
        logger.LogInformation("Email sent!");

        await client.DisconnectAsync(true);
        logger.LogInformation("SMTP client disconnected");
    }
}