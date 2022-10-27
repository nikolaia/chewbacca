using CvPartner.DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Refit;

using Shared;

namespace CvPartner.Api;

[ApiController]
[Route("[controller]")]
public class GetAllEmployees : ControllerBase
{
    private readonly IOptionsSnapshot<AppSettings> _appSettings;
    private readonly ILogger<GetAllEmployees> _logger;


    public GetAllEmployees(IOptionsSnapshot<AppSettings> appSettings, ILogger<GetAllEmployees> logger)
    {
        _appSettings = appSettings;
        _logger = logger;
    }

    public interface IEmployeeApi
    {
        [Get("/users?")]
        Task<IEnumerable<CVPartnerUserDTO>> GetAllEmployee([Authorize("Token")] string authorization);
    }


    [HttpGet]
    public async Task<IEnumerable<CVPartnerUserDTO>> Get()
    {
        _logger.LogInformation("Getting employees from database");
        var cvPartnerApi = RestService.For<IEmployeeApi>("https://variant.cvpartner.com/api/v1");
        var employees = await cvPartnerApi.GetAllEmployee(_appSettings.Value.Token);

        return employees;

        // var employees = _db.Employees.ToList();
        // return employees;
    }
}