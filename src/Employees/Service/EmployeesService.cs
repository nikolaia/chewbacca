using System.Net;
using System.Web;

using Employees.Models;
using Employees.Repositories;

using Microsoft.AspNetCore.Http;

namespace Employees.Service;

public class EmployeesService
{
    private readonly EmployeesRepository _employeesRepository;
    private readonly EmergencyContactRepository _emergencyContactRepository;
    private readonly EmployeeAllergiesAndDietaryPreferencesRepository _employeeAllergiesAndDietaryPreferencesRepository;

    public EmployeesService(EmployeesRepository employeesRepository,
        EmployeeAllergiesAndDietaryPreferencesRepository employeeAllergiesAndDietaryPreferencesRepository,
        EmergencyContactRepository emergencyContactRepository
        )
    {
        this._employeesRepository = employeesRepository;
        this._employeeAllergiesAndDietaryPreferencesRepository = employeeAllergiesAndDietaryPreferencesRepository;
        this._emergencyContactRepository = emergencyContactRepository;
    }

    private static bool IsEmployeeActive(EmployeeEntity employee)
    {
        return DateTime.Now > employee.StartDate &&
               (employee.EndDate == null || DateTime.Now < employee.EndDate);
    }

    /**
     * <returns>list of employees from database</returns>
     */
    public async Task<IEnumerable<EmployeeEntity>> GetActiveEmployees(string? country = null)
    {
        var employees = await (string.IsNullOrEmpty(country)
            ? _employeesRepository.GetAllEmployees()
            : _employeesRepository.GetEmployeesByCountry(country));

        return employees
            .Where(IsEmployeeActive);

    }

    public async Task<Employee?> GetByAliasAndCountry(string alias, string country)
    {
        var employee = await _employeesRepository.GetEmployeeAsync(alias, country);
        return employee == null ? null : ModelConverters.ToEmployee(employee);
    }

    public async Task<EmployeeEntity?> GetEntityByAliasAndCountry(string alias, string country)
    {
        return await _employeesRepository.GetEmployeeAsync(alias, country);
    }

    public Task AddOrUpdateEmployee(EmployeeEntity employee)
    {
        return _employeesRepository.AddToDatabase(employee);
    }

    public Task<string?> EnsureEmployeeIsDeleted(string email)
    {
        return _employeesRepository.EnsureEmployeeIsDeleted(email);
    }

    public Task<IEnumerable<string?>> EnsureEmployeesWithEndDateBeforeTodayAreDeleted()
    {
        return _employeesRepository.EnsureEmployeesWithEndDateBeforeTodayAreDeleted();
    }

    public async Task<bool> UpdateEmployeeInformationByAliasAndCountry(string alias, string country, EmployeeInformation employeeInformation)
    {
        return await _employeesRepository.UpdateEmployeeInformation(alias, country, employeeInformation);
    }

    public async Task<EmergencyContact?> GetEmergencyContactByEmployee(EmployeeEntity employee)
    {
        var emergencyContact = await _emergencyContactRepository.GetByEmployee(employee);

        return emergencyContact == null ? null : ModelConverters.ToEmergencyContact(emergencyContact);
    }

    public async Task<bool> AddOrUpdateEmergencyContactByAliasAndCountry(string alias, string country, EmergencyContact emergencyContact)
    {
        return await _emergencyContactRepository.AddOrUpdateEmergencyContact(alias, country, emergencyContact);
    }

    public Boolean isValid(EmergencyContact emergencyContact)
    {
        return emergencyContact.Name.Length >= 2 && emergencyContact.Phone.Length >= 8;
    }

    public List<DefaultAllergyEnum> GetDefaultAllergies()
    {
        return Enum.GetValues(typeof(DefaultAllergyEnum)).Cast<DefaultAllergyEnum>().ToList();
    }

    public List<DietaryPreferenceEnum> GetDietaryPreferences()
    {
        return Enum.GetValues(typeof(DietaryPreferenceEnum)).Cast<DietaryPreferenceEnum>().ToList();
    }

    public async Task<AllergiesAndDietaryPreferences?> GetAllergiesAndDietaryPreferencesByEmployee(EmployeeEntity employee)
    {
        var employeeAllergiesAndDietaryPreferences = await _employeeAllergiesAndDietaryPreferencesRepository.GetByEmployee(employee);

        return employeeAllergiesAndDietaryPreferences == null ? null : ModelConverters.ToAllergiesAndDietaryPreferences(employeeAllergiesAndDietaryPreferences);
    }

    public async Task<bool> UpdateAllergiesAndDietaryPreferencesByAliasAndCountry(string alias, string country, AllergiesAndDietaryPreferences allergiesAndDietaryPreferences)
    {
        return await _employeeAllergiesAndDietaryPreferencesRepository.AddOrUpdateEmployeeAllergiesAndDietaryPreferences(alias, country, allergiesAndDietaryPreferences);
    }

    public async Task<Cv> GetCvForEmployee(string alias, string country)
    {
        throw new BadHttpRequestException("service is not ready yet", 423);
        //må sette opp auth før denne kan brukes
        EmployeeEntity? employeeEntity = await _employeesRepository.GetEmployeeAsync(alias, country);
        if (employeeEntity == null)
        {
            throw new BadHttpRequestException($"Employee with alias {alias} and country {country} not found", 404);
        }
        var employeeId = employeeEntity.Id;
        List<PresentationEntity> presentationEntities = await _employeesRepository.GetPresentationsByEmployeeId(employeeId);
        List<WorkExperienceEntity> workExperienceEntities =
            await _employeesRepository.GetWorkExperiencesByEmployeeId(employeeId);
        List<ProjectExperienceEntity> projectExperienceEntities =
            await _employeesRepository.GetProjectExperiencesByEmployeeId(employeeId);

        return new Cv()
        {
            Presentations = presentationEntities.Select(ModelConverters.ToPresentation).ToList(),
            WorkExperiences = workExperienceEntities.Select(ModelConverters.ToWorkExperience).ToList(),
            ProjectExperiences = projectExperienceEntities.Select(ModelConverters.ToProjectExperience).ToList()
        };
    }
}