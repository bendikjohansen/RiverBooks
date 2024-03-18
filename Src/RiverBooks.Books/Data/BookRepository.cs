using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Books.Data;

internal interface IReadOnlyBookRepository
{
    Task<Book?> GetByIdAsync(Guid id);
    Task<List<Book>> ListAsync();
}
internal interface IBookRepository : IReadOnlyBookRepository
{
    Task AddAsync(Book book);
    Task DeleteAsync(Book book);
    Task SaveChangesAsync();
}

internal class EfBookRepository(BookDbContext dbContext) : IBookRepository
{
    public Task AddAsync(Book book)
    {
        dbContext.AddAsync(book);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Book book)
    {
        dbContext.Remove(book);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await dbContext.Books.FindAsync(id);
    }

    public Task<List<Book>> ListAsync()
    {
        return dbContext.Books.ToListAsync();
    }
}

public static class DataSchemaConstants
{
    public const int DefaultNameLength = 100;
}