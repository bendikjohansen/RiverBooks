using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RiverBooks.Books.Data;

using Serilog;

namespace RiverBooks.Books;

public static class BooksModuleServicesExtensions
{
    public static IServiceCollection AddBookModuleServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        List<Assembly> mediatrAssemblies)
    {
        var connectionString = configuration.GetConnectionString("Books") ?? string.Empty;
        services.AddDbContext<BookDbContext>(options => options.UseNpgsql(connectionString))
            .AddScoped<IBookRepository, EfBookRepository>()
            .AddScoped<IBookService, BookService>();
        mediatrAssemblies.Add(typeof(BooksModuleServicesExtensions).Assembly);

        logger.Information("{Module} module services registered", "Books");
        return services;
    }
}