using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace RiverBooks.EmailSending;

public static class EmailSendingModuleServiceExtensions
{
    public static IServiceCollection AddEmailSendingModuleServices(this IServiceCollection services, IConfiguration configuration, ILogger logger, List<Assembly> mediatrAssemblies)
    {
        services.AddTransient<ISendEmail, MimeKitEmailSender>();
        mediatrAssemblies.Add(typeof(EmailSendingModuleServiceExtensions).Assembly);

        logger.Information("{Module} module services registered", "EmailSending");

        return services;
    }
}