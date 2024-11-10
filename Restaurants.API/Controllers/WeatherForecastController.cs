using Microsoft.AspNetCore.Mvc;

namespace Restaurants.API.Controllers;

public class TemperatureRequest
{
    public int Min { get; set; }
    public int Max { get; set; }
}

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    [HttpPost("generate")]
    public IActionResult Generate([FromQuery] int count, [FromBody] TemperatureRequest temperatureRequest)
    {
        if(count <= 0 || temperatureRequest.Max < temperatureRequest.Min) 
            return BadRequest("Count has to be positive number and max temperature has to be greater than min temperature");

        var result = _weatherForecastService.Get(count, temperatureRequest.Min, temperatureRequest.Max);

        return Ok(result);
    }

}
