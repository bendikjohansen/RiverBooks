using Ardalis.Result;

using RiverBooks.Books.Data;

namespace RiverBooks.Books;

internal interface IBookService
{
    Task<List<BookDto>> ListBooksAsync();
    Task<Result<BookDto>> GetBookByIdAsync(Guid id);
    Task CreateBookAsync(BookDto newBook);
    Task DeleteBookAsync(Guid id);
    Task<Result> UpdateBookPriceAsync(Guid bookId, decimal newPrice);
}

internal class BookService(IBookRepository repository) : IBookService
{
    public async Task<List<BookDto>> ListBooksAsync()
    {
        var books = await repository.ListAsync();

        return books.Select(book => new BookDto(book.Id, book.Title, book.Author, book.Price)).ToList();
    }

    public async Task<Result<BookDto>> GetBookByIdAsync(Guid id)
    {
        var book = await repository.GetByIdAsync(id);

        if (book == null)
        {
            return Result.NotFound();
        }

        return new BookDto(book.Id, book.Title, book.Author, book.Price);
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

        if (book == null)
        {
            return;
        }

        await repository.DeleteAsync(book!);
        await repository.SaveChangesAsync();
    }

    public async Task<Result> UpdateBookPriceAsync(Guid bookId, decimal newPrice)
    {
        var book = await repository.GetByIdAsync(bookId);

        if (book is null)
        {
            return Result.NotFound();
        }

        book.UpdatePrice(newPrice);
        await repository.SaveChangesAsync();
        return Result.Success();
    }
}