using CvPartner.Api;
using Employee.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared;

namespace Employee.Api;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeContext _db;
    private readonly ILogger<EmployeeController> _logger;
    private readonly IOptionsSnapshot<AppSettings> _appSettings;
    private readonly ILogger<GetAllEmployees> _getAllEmployeesLogger;

    public EmployeeController(EmployeeContext db, ILogger<EmployeeController> logger,
        IOptionsSnapshot<AppSettings> appSettings, ILogger<GetAllEmployees> getAllEmployeesLogger)
    {
        _db = db;
        _logger = logger;
        _getAllEmployeesLogger = getAllEmployeesLogger;
        _appSettings = appSettings;
    }

    [HttpGet]
    public List<Models.Employee> Get()
    {
        _logger.LogInformation("Getting employees from database");
        var employees = _db.Employees.ToList();
        return employees;
    }

    [HttpPost]
    public async Task PostAllEmployees()
    {
        var service = new CvPartnerService.CvPartnerService(_appSettings, _getAllEmployeesLogger);
        var dataToDatabase = await service.FormatData();

        foreach (var employee in dataToDatabase)
        {
            _logger.LogInformation("Posting employye {0} to database", employee.Name);
            _db.Add(employee);
            _db.SaveChanges();
        }
    }
}