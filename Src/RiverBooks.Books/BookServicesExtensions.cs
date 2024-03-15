using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RiverBooks.Books;

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