using Employees.Models;

using Microsoft.AspNetCore.Mvc;
using Employees.Service;

using Microsoft.AspNetCore.OutputCaching;

namespace Employees.Api;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeesService _employeeService;
    
    public EmployeesController(EmployeesService employeeService)
    {
        this._employeeService = employeeService;
    }

    /**
     * <returns>a call to Service's GetAllEmployees</returns>
     */
    [HttpGet]
    [OutputCache(Duration = 60)]
    public async Task<EmployeeList> Get()
    {
        var employees = await _employeeService.GetAllEmployees();
        return new EmployeeList
        {
            Employees = employees.Where(employee => employee.StartDate < DateTime.Now)
        };
    }
}