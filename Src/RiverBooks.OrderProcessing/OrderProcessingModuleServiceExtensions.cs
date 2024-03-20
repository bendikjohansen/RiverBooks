using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RiverBooks.OrderProcessing.Data;

using Serilog;

namespace RiverBooks.OrderProcessing;

public static class OrderProcessingModuleServiceExtensions
{
    public static IServiceCollection AddOrderProcessingModuleServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        List<Assembly> mediatrAssemblies)
    {
        var connectionString = configuration.GetConnectionString("OrderProcessing");
        services.AddDbContext<OrderProcessingDbContext>(config => config.UseNpgsql(connectionString));
        mediatrAssemblies.Add(typeof(OrderProcessingModuleServiceExtensions).Assembly);
        services.AddScoped<IOrderRepository, EfOrderRepository>();
        services.AddScoped<RedisOrderAddressCache>();
        services.AddScoped<IOrderAddressCache, ReadThroughOrderAddressCache>();

        logger.Information("{Module} module services registered", "OrderProcessing");

        return services;
    }
}