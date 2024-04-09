using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RiverBooks.Reporting.Integrations;

using Serilog;

namespace RiverBooks.Reporting;

public static class ReportingModuleServicesExtensions
{
    public static IServiceCollection AddReportingModuleServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        List<Assembly> mediatrAssemblies)
    {
        // var connectionString = configuration.GetConnectionString("Books") ?? string.Empty;
        // services.AddDbContext<BookDbContext>(options => options.UseNpgsql(connectionString))
        services.AddScoped<OrderIngestionService>();
        services.AddScoped<IReportTopSalesByMonth, DefaultReportTopSalesByMonth>();

        mediatrAssemblies.Add(typeof(ReportingModuleServicesExtensions).Assembly);

        logger.Information("{Module} module services registered", "Books");
        return services;
    }
}