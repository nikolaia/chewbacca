using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces;

public interface IEmployeesRepository
{
    Task<List<Employee>> GetAllEmployees();
    Task<List<Employee>> GetEmployeesByCountry(string country);
    Task<Employee?> GetEmployeeAsync(string alias, string country);
    Task AddToDatabase(Employee employee);
    Task AddToDatabase(string email, EmergencyContact emergencyContact);

    Task<bool> UpdateEmployeeInformation(string alias, string country,
        EmployeeInformation employeeInformation);

    /// <summary>
    /// Deletes the employee from the database, if they exist, and returns the image url to the employees image blob that needs to be cleaned up
    /// </summary>
    /// <param name="email">Email of the employee</param>
    /// <returns>The image url to the employees image blob that needs to be cleaned up</returns>
    Task<string?> EnsureEmployeeIsDeleted(string email);

    Task<IEnumerable<string?>> EnsureEmployeesWithEndDateBeforeTodayAreDeleted();
    Task<EmergencyContact?> GetEmergencyContactAsync(Employee employee);
    
    // Task AddToDatabase(List<Presentation> presentations);
    // Task AddToDatabase(List<WorkExperience> presentations);
    // Task AddToDatabase(List<ProjectExperience> projectExperiences);
    // Task<List<Presentation>> GetPresentationsByEmployeeId(string alias, string country);
    // Task<List<WorkExperience>> GetWorkExperiencesByEmployeeId(string alias, string country);
    // Task<List<ProjectExperience>> GetProjectExperiencesByEmployeeId(string alias, string country);
}