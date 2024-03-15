using FastEndpoints;

namespace RiverBooks.Books;

internal class ListBooksEndpoint(IBookService bookService) : EndpointWithoutRequest<ListBooksResponse>
{
    public override void Configure()
    {
        Get("/books");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var books = await bookService.ListBooksAsync();

        await SendAsync(new ListBooksResponse(books), cancellation: ct);
    }
}

public record ListBooksResponse(List<BookDto> Books);