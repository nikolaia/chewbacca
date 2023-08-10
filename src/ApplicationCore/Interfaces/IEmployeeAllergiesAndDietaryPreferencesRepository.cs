using ApplicationCore.Models;

namespace ApplicationCore.Interfaces;

public interface IEmployeeAllergiesAndDietaryPreferencesRepository
{
    Task<EmployeeAllergiesAndDietaryPreferences?> GetByEmployee(string alias, string country);

    Task<bool> AddOrUpdateEmployeeAllergiesAndDietaryPreferences(string alias, string country,
        EmployeeAllergiesAndDietaryPreferences allergiesAndDietaryPreferences);
}