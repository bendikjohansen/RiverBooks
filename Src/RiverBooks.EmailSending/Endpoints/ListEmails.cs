using FastEndpoints;

using MongoDB.Driver;

namespace RiverBooks.EmailSending.Endpoints;

internal class ListEmails(IMongoCollection<EmailOutboxEntity> emailCollection) : EndpointWithoutRequest<ListEmailsResponse>
{
    public override void Configure()
    {
        Get("/emails");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var filter = Builders<EmailOutboxEntity>.Filter.Empty;
        var emailEntities = await emailCollection.Find(filter).ToListAsync(ct);

        var response = new ListEmailsResponse(emailEntities.Count, emailEntities);

        Response = response;
    }
}

public record ListEmailsResponse(int Count, List<EmailOutboxEntity> Emails);