using Employees.Models;
using Employees.Repositories;

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
    public async Task<IEnumerable<Employee>> GetActiveEmployees(string? country = null)
    {
        var employees = await (string.IsNullOrEmpty(country)
            ? _employeesRepository.GetAllEmployees()
            : _employeesRepository.GetEmployeesByCountry(country));

        return employees
            .Where(IsEmployeeActive)
            .Select(ModelConverters.ToEmployee);
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

    public Task UpdateEmployeeInformation(EmployeeEntity employee, EmployeeInformation employeeInformation)
    {
        employee.Telephone = employeeInformation.Phone;
        employee.Address = employeeInformation.Address;
        employee.AccountNumber = employeeInformation.AccountNumber;
        employee.ZipCode = employeeInformation.ZipCode;
        employee.City = employeeInformation.City;

        return _employeesRepository.AddToDatabase(employee);
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
}