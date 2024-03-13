using FastEndpoints;
using Microsoft.AspNetCore.Builder;

namespace RiverBooks.Books;

public static class BookEndpointsExtensions
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        app.MapGet("/books", (IBookService bookService) => bookService.ListBooks());
    }
}

internal class ListBooksEndpoint(IBookService bookService) : EndpointWithoutRequest<ListBooksResponse>
{
    public override void Configure()
    {
        Get("/api/books");
        AllowAnonymous();
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        return Task.FromResult(new ListBooksResponse(bookService.ListBooks()));
    }
}

public record ListBooksResponse(IEnumerable<BookDto> Books);
