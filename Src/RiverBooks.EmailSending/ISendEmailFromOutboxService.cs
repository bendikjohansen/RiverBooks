namespace RiverBooks.EmailSending;

internal interface ISendEmailFromOutboxService
{
    Task CheckForAndSendEmails();
}