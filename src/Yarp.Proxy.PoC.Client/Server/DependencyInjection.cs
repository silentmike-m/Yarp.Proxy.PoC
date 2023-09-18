namespace Yarp.Proxy.PoC.Client.Server;

using Yarp.Proxy.PoC.Client.Server.Services;

internal static class DependencyInjection
{
    public static void AddServer(this IServiceCollection services, IConfiguration configuration)
    {
        var serverOptions = configuration.GetSection(ServerOptions.SectionName).Get<ServerOptions>();
        serverOptions ??= new ServerOptions();

        services.AddHttpClient(Constants.HTTP_CLIENT_NAME, client =>
        {
            client.BaseAddress = serverOptions.BaseAddress;
        }).ConfigureHttpMessageHandlerBuilder(builder =>
        {
#if DEBUG
            builder.PrimaryHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
            };
#endif
        });

        services.AddScoped<ServerService>();
    }
}
