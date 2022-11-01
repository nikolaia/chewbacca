using Employee.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Shared;

namespace WebApi;

[ApiController]
[Route("[controller]")]
public class HealthcheckController : ControllerBase
{
    private readonly EmployeeContext _db;
    private readonly IOptionsSnapshot<AppSettings> _appSettings;
    private readonly ILogger<HealthcheckController> _logger;

    public HealthcheckController(EmployeeContext db, IOptionsSnapshot<AppSettings> appSettings,
        ILogger<HealthcheckController> logger)
    {
        _db = db;
        _appSettings = appSettings;
        _logger = logger;
    }

    [HttpGet]
    public async Task<JsonResult> Get()
    {
        _logger.LogInformation("Getting employees from database");

        var dbCanConnect = await _db.Database.CanConnectAsync();
        var healthcheck = _appSettings.Value.Healthcheck;

        var response = new
        {
            database = dbCanConnect,
            keyVault = healthcheck.KeyVault, 
            appConfig = healthcheck.AppConfig
        };
        
        return new JsonResult(response);
    }
}