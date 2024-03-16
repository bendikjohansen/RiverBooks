using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiverBooks.Books;
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

public class BookDbContext(DbContextOptions<BookDbContext> options) : DbContext(options)
{
    internal DbSet<Book> Books { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(nameof(Books));

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }
}

internal class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(p => p.Title)
            .HasMaxLength(DataSchemaConstants.DefaultNameLength)
            .IsRequired();

        builder.Property(p => p.Author)
            .HasMaxLength(DataSchemaConstants.DefaultNameLength)
            .IsRequired();

        builder.HasData(GetSampleBookData());
    }

    internal static readonly Guid Book1Id = new("DFFE455B-8F20-4B08-9EC5-3B4A1FFC4D18");
    internal static readonly Guid Book2Id = new("F8E9B841-E4AD-45ED-A68C-04E5EDD375FC");
    internal static readonly Guid Book3Id = new("BBB5A494-3CED-4E39-9BC9-59E1AC8123A1");

    private IEnumerable<Book> GetSampleBookData()
    {
        const string tolkien = "J.R.R. Tolkien";
        yield return new Book(Book1Id, "The Fellowship of the Ring", tolkien, 10.99M);
        yield return new Book(Book2Id, "The Two Towers", tolkien, 11.99M);
        yield return new Book(Book3Id, "The Return of the King", tolkien, 12.99M);
    }
}

public static class DataSchemaConstants
{
    public const int DefaultNameLength = 100;
}