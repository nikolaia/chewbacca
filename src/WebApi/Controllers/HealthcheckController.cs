using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthcheckController : ControllerBase
{
    private readonly ILogger<HealthcheckController> _logger;

    public HealthcheckController(ILogger<HealthcheckController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        _logger.LogInformation("Healthcheck ping received, sending pong!");
        return "pong";
    }
}
