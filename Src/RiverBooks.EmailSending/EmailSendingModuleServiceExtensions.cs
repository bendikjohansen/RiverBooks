using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

using RiverBooks.EmailSending.EmailBackgroundService;

using Serilog;

namespace RiverBooks.EmailSending;

public static class EmailSendingModuleServiceExtensions
{
    public static IServiceCollection AddEmailSendingModuleServices(this IServiceCollection services, IConfiguration configuration, ILogger logger, List<Assembly> mediatrAssemblies)
    {
        services.AddTransient<ISendEmail, MimeKitEmailSender>();
        services.AddTransient<IGetEmailsFromOutboxService, DefaultGetEmailsFromOutboxService>();
        services.AddTransient<IQueueEmailsForSendingService, DefaultQueueEmailsForSendingService>();
        services.AddTransient<ISendEmailFromOutboxService, DefaultSendEmailFromOutboxService>();

        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDB"));
        services.AddMongoDb(configuration);
        mediatrAssemblies.Add(typeof(EmailSendingModuleServiceExtensions).Assembly);

        // TODO: Find out why email client is not disconnecting properly
        // services.AddHostedService<EmailSendingBackgroundService>();

        logger.Information("{Module} module services registered", "EmailSending");

        return services;
    }

    private static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("MongoDB").Get<MongoDbSettings>() ??
                       throw new Exception("Missing MongoDB settings");
        services.AddSingleton<IMongoClient>(_ => new MongoClient(settings.ConnectionString));

        services.AddSingleton(serviceProvider =>
        {
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings.DatabaseName);
        });

        services.AddTransient(serviceProvider =>
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<EmailOutboxEntity>("EmailOutboxEntityCollection");
        });

        return services;
    }
}