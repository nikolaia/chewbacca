using Employees.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Employees.Service;

using Microsoft.AspNetCore.OutputCaching;

namespace Employees.Api;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeesService _employeeService;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(EmployeesService employeeService, ILogger<EmployeesController> logger)
    {
        this._employeeService = employeeService;
        this._logger = logger;
    }

    /**
     * <returns>a call to Service's GetAllEmployees</returns>
     */
    [HttpGet]
    [OutputCache(Duration = 60)]
    public async Task<EmployeesJson> Get([FromQuery] string? country = null)
    {
        var employees = await _employeeService.GetActiveEmployees(country);
        return new EmployeesJson
        {
            Employees = employees.Select(ModelConverters.ToEmployeeJson)
        };
    }

    /**
    * <returns>a call to Service's GetByNameAndCountry</returns>
    */
    [Microsoft.AspNetCore.Cors.EnableCors("DashCorsPolicy")]
    [HttpGet("{alias}/extended")]
    [OutputCache(Duration = 60)]
    public async Task<ActionResult<EmployeeExtendedJson>> GetExtendedByAlias(string alias, [FromQuery] string country)
    {
        var employee = await _employeeService.GetEntityByAliasAndCountry(alias, country);

        if (employee == null)
        {
            return NotFound();
        }

        var emergencyContact = await _employeeService.GetEmergencyContactByEmployee(employee);

        return ModelConverters.ToEmployeeExtendedJson(employee, emergencyContact);
    }

    /**
     * <returns>a call to Service's GetByNameAndCountry</returns>
     */
    [HttpGet("{alias}")]
    [OutputCache(Duration = 60)]
    public async Task<ActionResult<EmployeeJson>> GetByAlias(string alias, [FromQuery] string country)
    {
        var employee = await _employeeService.GetEntityByAliasAndCountry(alias, country);

        if (employee == null)
        {
            return NotFound();
        }
        else
        {
            return ModelConverters.ToEmployeeJson(employee);
        }
    }


    [Microsoft.AspNetCore.Cors.EnableCors("DashCorsPolicy")]
    [HttpPost("information/{country}/{alias}")]
    public async Task<ActionResult> UpdateEmployeeInformation(string alias, string country, [FromBody] EmployeeInformation employeeInformation)
    {
        var employee = await _employeeService.GetEntityByAliasAndCountry(alias, country);

        if (employee == null)
        {
            _logger.LogError("Can't update EmployeeInformation because there is no matching Employee to alias {alias} and country {country}", alias, country);
            return NotFound();
        }
        else
        {
            await _employeeService.UpdateEmployeeInformation(employee, employeeInformation);

            return NoContent();
        }
    }

    [Microsoft.AspNetCore.Cors.EnableCors("DashCorsPolicy")]
    [HttpPost("emergencyContact/{country}/{alias}")]
    public async Task<ActionResult> UpdateEmergencyContact(string alias, string country, [FromBody] EmergencyContact emergencyContact)
    {
        if (!_employeeService.isValid(emergencyContact))
        {
            return StatusCode(500, "Invalid data");
        }

        var employee = await _employeeService.GetEntityByAliasAndCountry(alias, country);

        if (employee == null)
        {
            _logger.LogError("Can't update EmergencyContact because there is no matching Employee to alias {alias} and country {country}", alias, country);
            return NotFound();
        }
        else
        {
            await _employeeService.AddOrUpdateEmergencyContact(employee, emergencyContact);
            return NoContent();
        }
    }
}