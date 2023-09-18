namespace Yarp.Proxy.PoC.Server.ViewModels;

using System.Text.Json.Serialization;

public class WeatherForecast
{
    [JsonPropertyName("date")] public DateTime Date { get; set; }

    [JsonPropertyName("summary")] public string? Summary { get; set; }

    [JsonPropertyName("temperature_c")] public int TemperatureC { get; set; }

    [JsonPropertyName("temperature_f")] public int TemperatureF => 32 + (int)(this.TemperatureC / 0.5556);
}
