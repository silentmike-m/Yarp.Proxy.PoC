namespace Yarp.Proxy.PoC.Client.ViewModels;

using System.Text.Json.Serialization;

public sealed record WeatherForecast
{
    [JsonPropertyName("date")] public DateTime Date { get; init; } = DateTime.MinValue;

    [JsonPropertyName("summary")] public string? Summary { get; init; } = default;

    [JsonPropertyName("temperature_c")] public int TemperatureC { get; init; } = default;

    [JsonPropertyName("temperature_f")] public int TemperatureF { get; init; } = default;
}
