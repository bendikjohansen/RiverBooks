namespace RiverBooks.Books;

internal interface IBookService
{
    ICollection<BookDto> ListBooks();
}

internal class BookService : IBookService
{
    public ICollection<BookDto> ListBooks() =>
    [
        new BookDto(Guid.NewGuid(), "The Fellowship of the Ring", "J.R.R. Tolkien"),
        new BookDto(Guid.NewGuid(), "The Two Towers", "J.R.R. Tolkien"),
        new BookDto(Guid.NewGuid(), "The Return of the King", "J.R.R. Tolkien"),
    ];
}
