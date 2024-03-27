using Microsoft.AspNetCore.Mvc;
using TestingIdentityApi.Services;

namespace TestingIdentityApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching", "getting"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IScopedProcessingService _scopedProcessingService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IScopedProcessingService scopedProcessingService)
        {
            _logger = logger;
            _scopedProcessingService = scopedProcessingService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("addtask", Name = "addtask")]
        public async Task<IActionResult> Addtask()
        {
            var task = _scopedProcessingService.SeedTask();
            return Ok(task);
        }
    }
}