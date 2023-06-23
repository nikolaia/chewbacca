using Employees.Models;
using Employees.Repositories;

namespace Employees.Service;

public class EmployeesService
{
    private readonly EmployeesRepository _employeesRepository;
    private readonly EmployeeDefaultAllergiesRepository _employeeDefaultAllergiesRepository;
    private readonly EmployeeOtherAllergiesRepository _employeeOtherAllergiesRepository;
    private readonly EmployeeDietaryPreferencesRepository _employeeDietaryPreferencesRepository;

    public EmployeesService(EmployeesRepository employeesRepository,
        EmployeeDefaultAllergiesRepository employeeDefaultAllergiesRepository,
        EmployeeOtherAllergiesRepository employeeOtherAllergiesRepository,
        EmployeeDietaryPreferencesRepository employeeDietaryPreferenceRepository)
    {
        this._employeesRepository = employeesRepository;
        this._employeeDefaultAllergiesRepository = employeeDefaultAllergiesRepository;
        this._employeeOtherAllergiesRepository = employeeOtherAllergiesRepository;
        this._employeeDietaryPreferencesRepository = employeeDietaryPreferenceRepository;
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
        var emergencyContact = await _employeesRepository.GetEmergencyContactAsync(employee);

        return emergencyContact == null ? null : ModelConverters.ToEmergencyContact(emergencyContact);
    }

    public Task AddOrUpdateEmergencyContact(EmployeeEntity employee, EmergencyContact emergencyContact)
    {
        var entity = new EmergencyContactEntity
        {
            Employee = employee,
            Name = emergencyContact.Name,
            Phone = emergencyContact.Phone,
            Relation = emergencyContact.Relation == "" ? null : emergencyContact.Relation,
            Comment = emergencyContact.Comment == "" ? null : emergencyContact.Comment,
        };

        return _employeesRepository.AddToDatabase(entity);
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

    public async Task<AllergiesAndDietaryPreferences> GetAllergiesAndDietaryPreferencesByEmployee(EmployeeEntity employee)
    {
        List<EmployeeDefaultAllergyEntity> defaultAllergies = await _employeeDefaultAllergiesRepository.GetByEmployee(employee);
        List<EmployeeOtherAllergyEntity> otherAllergies = await _employeeOtherAllergiesRepository.GetByEmployee(employee);
        List<EmployeeDietaryPreferenceEntity> dietaryPreferences = await _employeeDietaryPreferencesRepository.GetByEmployee(employee);

        return new AllergiesAndDietaryPreferences
        {
            DefaultAllergies = defaultAllergies.Select(a => a.DefaultAllergy.ToString()).ToList(),
            OtherAllergies = otherAllergies.Select(a => a.OtherAllergy).ToList(),
            DietaryPreferences = dietaryPreferences.Select(dp => dp.DietaryPreference.ToString()).ToList(),
            Comment = employee.AllergyComment == null ? "" : employee.AllergyComment
        };
    }

    public async Task UpdateDietaryPreferences(EmployeeEntity employee, List<string> dietaryPreferences)
    {
        List<DietaryPreferenceEnum> selectedDietaryPreferences = dietaryPreferences.ConvertAll(delegate (string dp) { return (DietaryPreferenceEnum)Enum.Parse(typeof(DietaryPreferenceEnum), dp); });
        List<EmployeeDietaryPreferenceEntity> existingDietaryPreferences = await _employeeDietaryPreferencesRepository.GetByEmployee(employee);

        foreach (var dietaryPreference in GetDietaryPreferences())
        {
            var existingDietaryPreference = existingDietaryPreferences.Find(existingDietaryPreference => existingDietaryPreference.DietaryPreference == dietaryPreference);

            if (existingDietaryPreference != null && !selectedDietaryPreferences.Contains(dietaryPreference))
            {
                await _employeeDietaryPreferencesRepository.Delete(existingDietaryPreference);
            }
            else if (existingDietaryPreference == null && selectedDietaryPreferences.Contains(dietaryPreference))
            {
                await AddEmployeeDietaryPreference(employee, dietaryPreference);
            }
        }
    }

    private async Task AddEmployeeDietaryPreference(EmployeeEntity employee, DietaryPreferenceEnum dietaryPreference)
    {
        await _employeeDietaryPreferencesRepository.AddToDatabase(employee, dietaryPreference);
    }

    public async Task UpdateAllergies(EmployeeEntity employee, List<string> defaultAllergies, List<string> otherAllergies)
    {
        List<DefaultAllergyEnum> selectedDefaultAllergies = defaultAllergies.ConvertAll(delegate (string allergy) { return (DefaultAllergyEnum)Enum.Parse(typeof(DefaultAllergyEnum), allergy); });
        List<EmployeeDefaultAllergyEntity> existingDefaultAllergies = await _employeeDefaultAllergiesRepository.GetByEmployee(employee);
        List<EmployeeOtherAllergyEntity> existingOtherAllergiesToEmployee = await _employeeOtherAllergiesRepository.GetByEmployee(employee);

        foreach (var defaultAllergy in GetDefaultAllergies())
        {
            var existingDefaultAllergy = existingDefaultAllergies.Find(allergy => allergy.DefaultAllergy == defaultAllergy);

            if (existingDefaultAllergy != null && !defaultAllergies.Contains(defaultAllergy.ToString()))
            {
                await _employeeDefaultAllergiesRepository.Delete(existingDefaultAllergy);
            }
            else if (existingDefaultAllergy == null && defaultAllergies.Contains(defaultAllergy.ToString()))
            {
                await AddEmployeeDefaultAllergy(employee, defaultAllergy);
            }
        }

        foreach (var otherAllergy in otherAllergies)
        {
            var existingOtherAllergy = existingOtherAllergiesToEmployee.Find(allergy => allergy.OtherAllergy == otherAllergy);

            if (existingOtherAllergy == null)
            {
                await AddEmployeeOtherAllergy(employee, otherAllergy);
            }
        }

        foreach (var existingOtherAllergy in existingOtherAllergiesToEmployee)
        {
            if (!otherAllergies.Contains(existingOtherAllergy.OtherAllergy))
            {
                await _employeeOtherAllergiesRepository.Delete(existingOtherAllergy);
            }
        }
    }

    private async Task AddEmployeeDefaultAllergy(EmployeeEntity employee, DefaultAllergyEnum defaultAllergy)
    {
        await _employeeDefaultAllergiesRepository.AddToDatabase(employee, defaultAllergy);
    }

    private async Task AddEmployeeOtherAllergy(EmployeeEntity employee, string otherAllergy)
    {
        await _employeeOtherAllergiesRepository.AddToDatabase(employee, otherAllergy);
    }

    public Task AddOrUpdateEmployeeAllergyComment(EmployeeEntity employee, string comment)
    {
        employee.AllergyComment = comment;
        return _employeesRepository.AddToDatabase(employee);
    }
}