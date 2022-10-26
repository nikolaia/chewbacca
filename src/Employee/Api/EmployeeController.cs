using CvPartner.DTOs;

using Employee.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Refit;

namespace Employee.Api;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeContext _db;
    private readonly ILogger<EmployeeController> _logger;
     public interface IEmployeeApi
    {
        [Headers("Authorization: Token token=6151d4be9aa0fc8f2e51367b57a9c78b")]
        [Get("/users?")]
        Task<IEnumerable<CVPartnerUserDTO>> GetAllEmployee();
    }

    public EmployeeController(EmployeeContext db, ILogger<EmployeeController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<CVPartnerUserDTO>> Get()
    {
        // _logger.LogInformation("Getting employees from database");
        var cvPartnerApi = RestService.For<IEmployeeApi>("https://variant.cvpartner.com/api/v1");
        var employees = await cvPartnerApi.GetAllEmployee();

        return employees;

        // var employees = _db.Employees.ToList();
        // return employees;
    }
}
