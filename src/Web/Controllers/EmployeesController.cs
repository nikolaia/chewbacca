using ApplicationCore.Models;
using ApplicationCore.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

using Web.ViewModels;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
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
    [AllowAnonymous]
    public async Task<EmployeesJson> Get([FromQuery] string? country = null)
    {
        var employees = await _employeeService.GetActiveEmployees(country);
        return new EmployeesJson { Employees = employees.Select(ModelConverters.ToEmployeeJson) };
    }

    /**
     * <returns>a call to Service's GetByNameAndCountry</returns>
     */
    [HttpGet("{alias}")]
    [OutputCache(Duration = 60)]
    [AllowAnonymous]
    public async Task<ActionResult<EmployeeJson>> GetByAlias(string alias, [FromQuery] string country)
    {
        var employee = await _employeeService.GetByAliasAndCountry(alias, country);

        if (employee == null)
        {
            return NotFound();
        }

        return ModelConverters.ToEmployeeJson(employee);
    }
    [HttpGet("cv")]
    [OutputCache(Duration = 60)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Cv> GetCvForEmployee ([FromQuery] string alias, [FromQuery] string country)
    {
        return await _employeeService.GetCvForEmployee(alias, country);
    }
    
    
    /// <summary>
    /// Returns all project for an employee which is relevant for at least one competency in the competency list
    /// </summary>
    [HttpGet("cv/projectExperiences")]
    [OutputCache(Duration = 60)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ProjectExperience>>> GetProjectsFromEmployee ([FromQuery] string alias, [FromQuery] string country, [FromQuery] List<string> competencies)
    {
        var employee = await _employeeService.GetByAliasAndCountry(alias, country);
        if (employee == null)
        {
            return NotFound();
        }
        return await _employeeService.GetProjectExperiencesForEmployee(employee.EmployeeInformation.Email, competencies);
    }
    

    /**
    * <returns>a call to Service's GetByNameAndCountry</returns>
    */
    [EnableCors("DashCorsPolicy")]
    [HttpGet("{alias}/extended")]
    [OutputCache(Duration = 60)]
    public async Task<ActionResult<EmployeeExtendedJson>> GetExtendedByAlias(string alias, [FromQuery] string country)
    {
        var employee = await _employeeService.GetByAliasAndCountry(alias, country);

        if (employee == null)
        {
            return NotFound();
        }
        return ModelConverters.ToEmployeeExtendedJson(employee);
    }

    [EnableCors("DashCorsPolicy")]
    [HttpPost("information/{country}/{alias}")]
    public async Task<ActionResult> UpdateEmployeeInformation(string alias, string country,
        [FromBody] UpdateEmployeeInformation employeeInformation)
    {
        var updateSuccess =
            await _employeeService.UpdateEmployeeInformationByAliasAndCountry(alias, country, employeeInformation);
        if (updateSuccess)
        {
            return NoContent();
        }

        _logger.LogError(
            "Can't update EmployeeInformation because there is no matching Employee to alias {alias} and country {country}",
            alias, country);
        return NotFound();
    }


    [EnableCors("DashCorsPolicy")]
    [HttpPost("emergencyContact/{country}/{alias}")]
    public async Task<ActionResult> UpdateEmergencyContact(string alias, string country,
        [FromBody] EmergencyContact emergencyContact)
    {
        if (!_employeeService.isValid(emergencyContact))
        {
            return StatusCode(500, "Invalid data");
        }

        var updateSuccess =
            await _employeeService.AddOrUpdateEmergencyContactByAliasAndCountry(alias, country, emergencyContact);

        if (updateSuccess)
        {
            return NoContent();
        }

        _logger.LogError(
            "Can't update EmergencyContact because there is no matching Employee to alias {alias} and country {country}",
            alias, country);
        return NotFound();
    }

    /**
     * <returns>A list of the allergies as strings</returns>
     */
    [HttpGet("allergies")]
    [OutputCache(Duration = 60)]
    public List<string> GetAllergies()
    {
        return _employeeService.GetDefaultAllergies().Select(a => a.ToString()).ToList();
    }

    /**
     * <returns>A list of the dietary preferences as strings</returns>
     */
    [HttpGet("dietaryPreferences")]
    [OutputCache(Duration = 60)]
    public List<string> GetDietaryPreferences()
    {
        return _employeeService.GetDietaryPreferences().Select(a => a.ToString()).ToList();
    }

    /// <summary>
    /// Returns all competencies that Variant has
    /// </summary>
    [HttpGet("competencies")]
    [OutputCache(Duration = 60)]
    public async Task<List<string>> GetAllCompetencies()
    {
        return await _employeeService.GetAllCompetencies();
    }

    [EnableCors("DashCorsPolicy")]
    [HttpPost("allergiesAndDietaryPreferences/{country}/{alias}")]
    public async Task<IActionResult> UpdateAllergiesAndDietaryPreferences(string alias, string country,
        [FromBody] EmployeeAllergiesAndDietaryPreferencesJson allergiesAndDietaryPreferencesJson)
    {
        var allergiesAndDietaryPreferences = new EmployeeAllergiesAndDietaryPreferences
        {
            Comment = allergiesAndDietaryPreferencesJson.Comment,
            DefaultAllergies =
                ModelConverters.DefaultAllergyStringListToEnumList(allergiesAndDietaryPreferencesJson
                    .DefaultAllergies),
            DietaryPreferences =
                ModelConverters.DietaryPreferenceStringListToEnumList(allergiesAndDietaryPreferencesJson
                    .DietaryPreferences),
            OtherAllergies = allergiesAndDietaryPreferencesJson.OtherAllergies
        };

        var updateSuccess =
            await _employeeService.UpdateAllergiesAndDietaryPreferencesByAliasAndCountry(alias, country,
                allergiesAndDietaryPreferences);

        if (updateSuccess)
        {
            return NoContent();
        }

        _logger.LogWarning(
            "Can't update allergies and dietary preferences because there is no matching Employee to alias {Alias} and country {Country}",
            alias, country);
        return NotFound();
    }
}