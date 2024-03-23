using FastEndpoints;

namespace RiverBooks.Books.BookEndpoints;

public record DeleteBookRequest(Guid Id);

internal class Delete(IBookService bookService) : Endpoint<DeleteBookRequest, BookDto>
{
    public override void Configure()
    {
        Delete("/books/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteBookRequest req, CancellationToken ct)
    {
        await bookService.DeleteBookAsync(req.Id);
        await SendNoContentAsync(ct);
    }
}