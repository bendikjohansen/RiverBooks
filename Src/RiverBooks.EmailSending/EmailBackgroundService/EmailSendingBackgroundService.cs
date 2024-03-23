using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RiverBooks.EmailSending.EmailBackgroundService;

internal class EmailSendingBackgroundService(ISendEmailFromOutboxService sendEmailFromOutboxService, ILogger<EmailSendingBackgroundService> logger) : BackgroundService
{
    private const int DelayMilliseconds = 3_000;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        logger.LogInformation("{serviceName} starting...", nameof(EmailSendingBackgroundService));

        while (!ct.IsCancellationRequested)
        {
            try
            {
                await sendEmailFromOutboxService.CheckForAndSendEmails(ct);
            }
            catch (Exception exception)
            {
                logger.LogError("Error processing outbox: {message}", exception.Message);
            }
            finally
            {
                await Task.Delay(DelayMilliseconds, ct);
            }

        }

        logger.LogInformation("{serviceName} stopping...", nameof(EmailSendingBackgroundService));
    }
}