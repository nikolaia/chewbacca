using Employee.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Employee.Api;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeContext _db;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(EmployeeContext db, ILogger<EmployeeController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpGet]
    public List<Models.Employee> Get()
    {
        _logger.LogInformation("Getting employees from database");
        
        var employees = _db.Employees.ToList();
        return employees;
    }
}
