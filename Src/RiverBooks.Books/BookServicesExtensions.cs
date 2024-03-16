using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace RiverBooks.Books;

public static class BookServicesExtensions
{
    public static IServiceCollection AddBookModuleServices(this IServiceCollection services, ConfigurationManager config, ILogger logger)
    {
        var connectionString = config.GetConnectionString("Books") ?? string.Empty;
        services.AddDbContext<BookDbContext>(options => options.UseNpgsql(connectionString))
            .AddScoped<IBookRepository, EfBookRepository>()
            .AddScoped<IBookService, BookService>();

        logger.Information("{Module} module services registered", "Books");
        return services;
    }
}