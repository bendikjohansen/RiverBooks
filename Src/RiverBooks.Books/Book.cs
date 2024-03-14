using System.Reflection;

using Ardalis.GuardClauses;

using FastEndpoints;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

public class BookDbContext(DbContextOptions options) : DbContext(options)
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

internal record Book
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    internal Book(Guid id, string title, string author, decimal price)
    {
        Id = Guard.Against.Default(id);
        Title = Guard.Against.NullOrEmpty(title);
        Author = Guard.Against.NullOrEmpty(author);
        Price = Guard.Against.Negative(price);
    }

    internal void UpdatePrice(decimal price) => Price = Guard.Against.Negative(price);
}

public record BookDto(Guid Id, string Title, string Author, decimal Price);

public static class BookServicesExtensions
{
    public static IServiceCollection AddBookServices(this IServiceCollection services, ConfigurationManager config)
    {
        var connectionString = config.GetConnectionString("Books") ?? string.Empty;
        return services.AddDbContext<BookDbContext>(options => options.UseNpgsql(connectionString))
            .AddScoped<IBookRepository, EfBookRepository>()
            .AddScoped<IBookService, BookService>();
    }
}