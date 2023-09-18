namespace Yarp.Proxy.PoC.Client.Server.Services;

using Yarp.Proxy.PoC.Client.ViewModels;

public sealed class ServerService
{
    private const string GET_WEATHER_FORECAST_ENDPOINT = "WeatherForecast/GetWeatherForecast";
    private const string HTTP_POST_PING_ENDPOINT = "WeatherForecast/PostPing";

    private readonly HttpClient httpClient;

    public ServerService(IHttpClientFactory httpClientFactory)
        => this.httpClient = httpClientFactory.CreateClient(Constants.HTTP_CLIENT_NAME);

    public async Task<IReadOnlyList<WeatherForecast>?> GetWeatherForecast(CancellationToken cancellationToken = default)
        => await this.httpClient.GetFromJsonAsync<IReadOnlyList<WeatherForecast>>(GET_WEATHER_FORECAST_ENDPOINT, cancellationToken);

    public async Task PostPing(CancellationToken cancellationToken = default)
    {
        var response = await this.httpClient.PostAsync(HTTP_POST_PING_ENDPOINT, content: null, cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
