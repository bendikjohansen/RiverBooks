using FastEndpoints;
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();

builder.Services
    .AddBookModuleServices(builder.Configuration, logger)
    .AddUserModuleServices(builder.Configuration, logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();

app.Run();

public partial class Program {} // needed for tests