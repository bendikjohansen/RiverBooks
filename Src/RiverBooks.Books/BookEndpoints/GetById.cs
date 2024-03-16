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
        var book = await bookService.GetBookByIdAsync(req.Id);

        if (book is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        await SendAsync(book, cancellation: ct);
    }
}