using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RiverBooks.EmailSending;

internal class EmailSendingBackgroundService(ISendEmailFromOutboxService sendEmailFromOutboxService, ILogger<EmailSendingBackgroundService> logger) : BackgroundService
{
    private const int DelayMilliseconds = 3_000;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{serviceName} starting...", nameof(EmailSendingBackgroundService));

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await sendEmailFromOutboxService.CheckForAndSendEmails();
            }
            catch (Exception exception)
            {
                logger.LogError("Error processing outbox: {message}", exception.Message);
            }
            finally
            {
                await Task.Delay(DelayMilliseconds, stoppingToken);
            }

            logger.LogInformation("{serviceName} stopping...", nameof(EmailSendingBackgroundService));
        }
    }
}