using Database.Seed.Entities;

using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

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
    public List<Employee> Get()
    {
        _logger.LogInformation("Getting employees from database");
        
        var employees = _db.Employees.ToList();
        return employees;
    }
}
