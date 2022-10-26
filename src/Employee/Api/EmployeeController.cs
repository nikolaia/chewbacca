using CvPartner.DTOs;

using Employee.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Refit;

namespace Employee.Api;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeContext _db;
    private readonly ILogger<EmployeeController> _logger;
    private readonly IConfiguration _congig;
     public interface IEmployeeApi
    {
        [Get("/users?")]
        Task<IEnumerable<CVPartnerUserDTO>> GetAllEmployee([Authorize("Token")] string authorization);
    }

    public EmployeeController(EmployeeContext db, ILogger<EmployeeController> logger, IConfiguration config)
    {
        _db = db;
        _logger = logger;
        _congig = config;
    }

    [HttpGet]
    public async Task<IEnumerable<CVPartnerUserDTO>> Get()
    {
        // _logger.LogInformation("Getting employees from database");
        var apikey = _congig["Employee:ServiceApiKey"];

        Console.WriteLine("HER");
        Console.WriteLine("skal stå her: " + apikey);
        var cvPartnerApi = RestService.For<IEmployeeApi>("https://variant.cvpartner.com/api/v1");
        var employees = await cvPartnerApi.GetAllEmployee(apikey);

        return employees;

        // var employees = _db.Employees.ToList();
        // return employees;
    }
}
