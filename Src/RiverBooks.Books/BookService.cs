using RiverBooks.Books.Data;

namespace RiverBooks.Books;

internal interface IBookService
{
    Task<List<BookDto>> ListBooksAsync();
    Task<BookDto> GetBookByIdAsync(Guid id);
    Task CreateBookAsync(BookDto newBook);
    Task DeleteBookAsync(Guid id);
    Task UpdateBookPriceAsync(Guid bookId, decimal newPrice);
}

internal class BookService(IBookRepository repository) : IBookService
{
    public async Task<List<BookDto>> ListBooksAsync()
    {
        var books = await repository.ListAsync();

        return books.Select(book => new BookDto(book.Id, book.Title, book.Author, book.Price)).ToList();
    }

    public async Task<BookDto> GetBookByIdAsync(Guid id)
    {
        var book = await repository.GetByIdAsync(id);

        // TODO: handle not found case

        return new BookDto(book!.Id, book.Title, book.Author, book.Price);
    }

    public async Task CreateBookAsync(BookDto newBook)
    {
        var book = new Book(newBook.Id, newBook.Title, newBook.Author, newBook.Price);

        await repository.AddAsync(book);
        await repository.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(Guid id)
    {
        var book = await repository.GetByIdAsync(id);

        // TODO: handle not found case

        await repository.DeleteAsync(book!);
        await repository.SaveChangesAsync();
    }

    public async Task UpdateBookPriceAsync(Guid bookId, decimal newPrice)
    {
        // TODO: validate the price

        var book = await repository.GetByIdAsync(bookId);

        // TODO: handle no case found

        book!.UpdatePrice(newPrice);
        await repository.SaveChangesAsync();
    }
}