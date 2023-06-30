using Employees.Models;

using Microsoft.EntityFrameworkCore;

namespace Employees.Repositories;

public class EmployeeAllergiesAndDietaryPreferencesRepository
{
    private readonly EmployeeContext _db;
    private readonly EmployeesRepository _employeesRepository;

    public EmployeeAllergiesAndDietaryPreferencesRepository(EmployeeContext db, EmployeesRepository employeesRepository)
    {
        _db = db;
        _employeesRepository = employeesRepository;
    }

    public async Task<EmployeeAllergiesAndDietaryPreferencesEntity?> GetByEmployee(EmployeeEntity employee)
    {
        return await _db.EmployeeAllergiesAndDietaryPreferences
            .Where(e => e.Employee.Equals(employee))
            .SingleOrDefaultAsync();
    }

    public async Task<bool> AddOrUpdateEmployeeAllergiesAndDietaryPreferences(string alias, string country,
        AllergiesAndDietaryPreferences allergiesAndDietaryPreferences)
    {
        var employee = await _employeesRepository.GetEmployeeAsync(alias, country);
        
        if (employee == null)
        {
            return false;
        }
        
        employee.AllergiesAndDietaryPreferences = new EmployeeAllergiesAndDietaryPreferencesEntity
        {
            Employee = employee,
            DefaultAllergies =
                ModelConverters.DefaultAllergyStringListToEnumList(allergiesAndDietaryPreferences.DefaultAllergies),
            OtherAllergies = allergiesAndDietaryPreferences.OtherAllergies,
            DietaryPreferences =
                ModelConverters.DietaryPreferenceStringListToEnumList(allergiesAndDietaryPreferences
                    .DietaryPreferences),
            Comment = allergiesAndDietaryPreferences.Comment
        };

        var changes = await _db.SaveChangesAsync();
        return changes > 0;
    }

    public async Task Delete(EmployeeAllergiesAndDietaryPreferencesEntity employeeAllergy)
    {
        _db.Remove(employeeAllergy);
        await _db.SaveChangesAsync();
    }
}