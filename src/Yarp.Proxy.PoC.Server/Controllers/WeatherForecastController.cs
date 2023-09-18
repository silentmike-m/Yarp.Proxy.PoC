namespace Yarp.Proxy.PoC.Server.Controllers;

using Microsoft.AspNetCore.Mvc;
using Yarp.Proxy.PoC.Server.ViewModels;

[ApiController, Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] SUMMARIES =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
    };

    private readonly ILogger<WeatherForecastController> logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        this.logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
    {
        this.logger.LogInformation("Received get weathers forecast request");

        var result = Enumerable.Range(start: 1, count: 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(minValue: -20, maxValue: 55),
                Summary = SUMMARIES[Random.Shared.Next(SUMMARIES.Length)],
            })
            .ToArray();

        return await Task.FromResult(result);
    }

    [HttpPost(Name = "PostPing")]
    public async Task<IActionResult> PostPing()
    {
        this.logger.LogInformation("Received post ping request");

        return await Task.FromResult<IActionResult>(this.Ok());
    }
}
