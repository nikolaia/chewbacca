using Employees.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Employees.Service;
using Employees.Models;

namespace Employees.Api;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeesService _employeeService;
    
    public EmployeeController(EmployeesService _employeeService)
    {
        this._employeeService = _employeeService;
    }

    [HttpGet]
    public async Task<IEnumerable<Employee>> Get()
    {
        // Hente Employees fra EmployeesSevice
        // Returner
        return await _employeeService.GetAllEmployees();
    }
}