using Microsoft.Extensions.DependencyInjection;

namespace RiverBooks.Books;

public static class BookServicesExtensions
{
    public static IServiceCollection AddBookServices(this IServiceCollection services) =>
        services.AddScoped<IBookService, BookService>();
}