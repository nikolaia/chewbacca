using ApplicationCore.Models;

namespace ApplicationCore.Interfaces;

public interface IEmployeesRepository
{
    Task<List<Employee>> GetAllEmployees();
    Task<List<Employee>> GetEmployeesByCountry(string country);
    Task<Employee?> GetEmployeeAsync(string alias, string country);
    Task AddOrUpdateEmployeeInformation(Employee employee);

    Task<bool> UpdateEmployeeInformation(string alias, string country,
        UpdateEmployeeInformation employeeInformation);

    /// <summary>
    /// Deletes the employee from the database, if they exist, and returns the image url to the employees image blob that needs to be cleaned up
    /// </summary>
    /// <param name="email">Email of the employee</param>
    /// <returns>The image url to the employees image blob that needs to be cleaned up</returns>
    Task<string?> EnsureEmployeeIsDeleted(string email);

    Task<IEnumerable<string?>> EnsureEmployeesWithEndDateBeforeTodayAreDeleted();

    Task AddOrUpdateCvInformation(List<Cv> cvs);

    Task<Cv> GetEmployeeWithCv(string alias, string country);

    Task<List<ProjectExperience>> GetProjectExperiencesByEmailAndCompetencies(string email, List<string> competencies);
    Task<List<string>> GetAllCompetencies();
    Task<List<string>> GetAllCompetenciesForEmployee(string email);
}