using Employees.Repositories;
using Microsoft.AspNetCore.Mvc;
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

    /**
     * <returns>a call to Service's GetAllEmployees</returns>
     */
    [HttpGet]
    public async Task<IEnumerable<Employee>> Get()
    {
        return await _employeeService.GetAllEmployees();
    }
}