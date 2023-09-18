using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Yarp.Proxy.PoC.Client;
using Yarp.Proxy.PoC.Client.Server;

const int EXIT_FAILURE = 1;
const int EXIT_SUCCESS = 0;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("CONFIG_");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);

builder.Services.AddServer(builder.Configuration);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

var corsOptions = builder.Configuration.GetSection(CorsOptions.SECTION_NAME).Get<CorsOptions>();
corsOptions ??= new CorsOptions();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy",
        policyBuilder => policyBuilder
            .WithOrigins(corsOptions.AllowedOrigins)
            .AllowCredentials()
            .WithHeaders(corsOptions.AllowedHeaders)
            .WithMethods(corsOptions.AllowedMethods));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

try
{
    Log.Information("Starting host...");

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.MapControllers();

    await app.RunAsync();

    return EXIT_SUCCESS;
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly");

    return EXIT_FAILURE;
}
finally
{
    Log.CloseAndFlush();
}
