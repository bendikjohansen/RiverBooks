using FastEndpoints;

namespace RiverBooks.Books.BookEndpoints;

public record CreateBookRequest
{
    public Guid? Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public decimal Price { get; init; }
}

internal class Create(IBookService bookService) : Endpoint<CreateBookRequest, BookDto>
{
    public override void Configure()
    {
        Post("/books");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBookRequest req, CancellationToken ct)
    {
        var newBook = new BookDto(req.Id ?? new Guid(), req.Title, req.Author, req.Price);
        await bookService.CreateBookAsync(newBook);
        await SendCreatedAtAsync<GetById>(new {newBook.Id}, newBook, cancellation: ct);
    }
}