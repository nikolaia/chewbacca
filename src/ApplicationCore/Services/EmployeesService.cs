using ApplicationCore.Interfaces;
using ApplicationCore.Models;

namespace ApplicationCore.Services;

public class EmployeesService
{
    private readonly IEmployeesRepository _employeesRepository;
    private readonly IEmergencyContactRepository _emergencyContactRepository;

    private readonly IEmployeeAllergiesAndDietaryPreferencesRepository
        _employeeAllergiesAndDietaryPreferencesRepository;

    public EmployeesService(IEmployeesRepository employeesRepository,
        IEmployeeAllergiesAndDietaryPreferencesRepository employeeAllergiesAndDietaryPreferencesRepository,
        IEmergencyContactRepository emergencyContactRepository
    )
    {
        this._employeesRepository = employeesRepository;
        this._employeeAllergiesAndDietaryPreferencesRepository = employeeAllergiesAndDietaryPreferencesRepository;
        this._emergencyContactRepository = emergencyContactRepository;
    }

    private static bool IsEmployeeActive(Employee employee)
    {
        return DateTime.Now > employee.EmployeeInformation.StartDate &&
               (employee.EmployeeInformation.EndDate == null || DateTime.Now < employee.EmployeeInformation.EndDate);
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
            .Where(IsEmployeeActive);
    }

    public async Task<Employee?> GetByAliasAndCountry(string alias, string? country = null)
    {
        return await _employeesRepository.GetEmployeeAsync(alias, country);
    }

    public Task AddOrUpdateEmployee(Employee employee)
    {
        return _employeesRepository.AddOrUpdateEmployeeInformation(employee);
    }

    public Task<string?> EnsureEmployeeIsDeleted(string email)
    {
        return _employeesRepository.EnsureEmployeeIsDeleted(email);
    }

    public async Task<EmergencyContact?> GetEmergencyContactByEmployee(string alias, string country)
    {
        return await _emergencyContactRepository.GetByEmployee(alias, country);
    }

    public async Task<bool> AddOrUpdateEmergencyContactByAliasAndCountry(string alias, string country,
        EmergencyContact emergencyContact)
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

    public async Task<EmployeeAllergiesAndDietaryPreferences?> GetAllergiesAndDietaryPreferencesByEmployee(string alias,
        string country)
    {
        return await _employeeAllergiesAndDietaryPreferencesRepository.GetByEmployee(alias, country);
    }

    public async Task<bool> UpdateAllergiesAndDietaryPreferencesByAliasAndCountry(string alias, string country,
        EmployeeAllergiesAndDietaryPreferences allergiesAndDietaryPreferences)
    {
        return await
            _employeeAllergiesAndDietaryPreferencesRepository.AddOrUpdateEmployeeAllergiesAndDietaryPreferences(alias,
                country, allergiesAndDietaryPreferences);
    }

    public async Task<Cv> GetCvForEmployee(string alias, string country)
    {
        return await _employeesRepository.GetEmployeeWithCv(alias, country);
    }

    public async Task<ProjectExprienceResponse> GetProjectExperiencesForEmployee(string email,
        List<string> competencies)
    {
        List<ProjectExperience> relevantProjects =  await _employeesRepository.GetProjectExperiencesByEmailAndCompetencies(email, competencies);
        return new ProjectExprienceResponse{
            projects = relevantProjects,
            MonthsOfExperience = relevantProjects.Select( pe => CalculateMonths(pe.FromDate, pe.ToDate)).Sum()
        };
    }

    public async Task<List<string>> GetAllCompetencies(string? email = null)
    {
        return await _employeesRepository.GetAllCompetencies(email);
    }
    private static int CalculateMonths(DateOnly? from, DateOnly to){
        if(from == null){
            return 0;
        }
        return ((to.Year - from.Value.Year) * 12) + (to.Month - from.Value.Month) + 1;
    }
}