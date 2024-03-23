using Ardalis.Result;

using FastEndpoints;

namespace RiverBooks.Books.BookEndpoints;

public record GetByIdRequest(Guid Id);

internal class GetById(IBookService bookService) : Endpoint<GetByIdRequest, BookDto>
{
    public override void Configure()
    {
        Get("/books/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetByIdRequest req, CancellationToken ct)
    {
        var result = await bookService.GetBookByIdAsync(req.Id);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        await SendAsync(result.Value, cancellation: ct);
    }
}