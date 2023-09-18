using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Yarp.Proxy.PoC.WebApi;

const int EXIT_FAILURE = 1;
const int EXIT_SUCCESS = 0;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("proxy.json")
    .AddEnvironmentVariables("CONFIG_");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

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

try
{
    Log.Information("Starting host...");

    var app = builder.Build();

    app.UseHttpsRedirection();

    app.MapReverseProxy();

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
