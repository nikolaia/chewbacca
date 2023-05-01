using Employees.Models;

using Microsoft.AspNetCore.Mvc;
using Employees.Service;

using Microsoft.AspNetCore.OutputCaching;

namespace Employees.Api;

[ApiController]
[Route("{country}/[controller]")]
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
        var employees = await _employeeService.GetActiveEmployeesInCountry();
        return new EmployeesJson
        {
            Employees = employees.Select(ModelConverters.ToEmployeeJson)
        };
    }

    /**
     * <returns>a call to Service's GetByNameAndCountry</returns>
     */
    [HttpGet("{alias}")]
    [OutputCache(Duration = 60)]
    public async Task<ActionResult<EmployeeJson>> GetByName(string alias)
    {
        var employee = await _employeeService.GetByAliasAndCountry(alias);
        if (employee == null)
        {
            return NotFound();
        }
        return ModelConverters.ToEmployeeJson(employee);
    }
}