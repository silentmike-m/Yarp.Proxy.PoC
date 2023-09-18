namespace Yarp.Proxy.PoC.Client.Controllers;

using Microsoft.AspNetCore.Mvc;
using Yarp.Proxy.PoC.Client.Server.Services;
using Yarp.Proxy.PoC.Client.ViewModels;

[ApiController, Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ServerService serverService;

    public WeatherForecastController(ServerService serverService)
        => this.serverService = serverService;

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
    {
        var result = await this.serverService.GetWeatherForecast();

        if (result is null)
        {
            throw new Exception("Not found");
        }

        return result;
    }

    [HttpPost(Name = "PostPing")]
    public async Task<IActionResult> PostPing()
    {
        await this.serverService.PostPing();

        return this.Ok();
    }
}
