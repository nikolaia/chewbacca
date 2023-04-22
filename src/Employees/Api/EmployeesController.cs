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
    public async Task<EmployeesJson> Get()
    {
        var employees = await _employeeService.GetAllActiveEmployees();
        return new EmployeesJson
        {
            Employees = employees.Select(ModelConverters.ToEmployeeJson)
        };
    }
}