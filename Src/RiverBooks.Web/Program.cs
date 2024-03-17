using System.Reflection;

using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;

using RiverBooks.Books;
using RiverBooks.Users;

using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((_, config) =>
    config.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddFastEndpoints()
    .AddAuthenticationJwtBearer(o => o.SigningKey = builder.Configuration["Auth:JwtSecret"]!)
    .AddAuthorization()
    .SwaggerDocument();

List<Assembly> mediatrAssemblies = [typeof(Program).Assembly];
builder.Services
    .AddBookModuleServices(builder.Configuration, logger, mediatrAssemblies)
    .AddUserModuleServices(builder.Configuration, logger, mediatrAssemblies);

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(mediatrAssemblies.ToArray()));

var app = builder.Build();

app.UseAuthentication()
    .UseAuthorization();

app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();

public partial class Program {} // needed for tests