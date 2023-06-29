using Employees.Models;

using Microsoft.EntityFrameworkCore;

namespace Employees.Repositories;

public class EmployeeAllergiesAndDietaryPreferencesRepository
{
    private readonly EmployeeContext _db;

    public EmployeeAllergiesAndDietaryPreferencesRepository(EmployeeContext db)
    {
        _db = db;
    }

    public async Task<EmployeeAllergiesAndDietaryPreferencesEntity?> GetByEmployee(EmployeeEntity employee)
    {
        return await _db.EmployeeAllergiesAndDietaryPreferences
            .Where(e => e.Employee.Equals(employee))
            .SingleOrDefaultAsync();
    }

    public async Task AddOrUpdateToDatabase(EmployeeEntity employee, AllergiesAndDietaryPreferences allergiesAndDietaryPreferences)
    {
        EmployeeAllergiesAndDietaryPreferencesEntity? existingEmployeeAllergiesAndDietaryPreferences = await GetByEmployee(employee);

        if (existingEmployeeAllergiesAndDietaryPreferences == null)
        {
            var employeeAllergyAndDietaryPreferences = new EmployeeAllergiesAndDietaryPreferencesEntity
            {
                Employee = employee,
                DefaultAllergies = ModelConverters.DefaultAllergyStringListToEnumList(allergiesAndDietaryPreferences.DefaultAllergies),
                OtherAllergies = allergiesAndDietaryPreferences.OtherAllergies,
                DietaryPreferences = ModelConverters.DietaryPreferenceStringListToEnumList(allergiesAndDietaryPreferences.DietaryPreferences),
                Comment = allergiesAndDietaryPreferences.Comment
            };

            await _db.AddAsync(employeeAllergyAndDietaryPreferences);
        }
        else
        {
            existingEmployeeAllergiesAndDietaryPreferences.DefaultAllergies = ModelConverters.DefaultAllergyStringListToEnumList(allergiesAndDietaryPreferences.DefaultAllergies);
            existingEmployeeAllergiesAndDietaryPreferences.OtherAllergies = allergiesAndDietaryPreferences.OtherAllergies;
            existingEmployeeAllergiesAndDietaryPreferences.DietaryPreferences = ModelConverters.DietaryPreferenceStringListToEnumList(allergiesAndDietaryPreferences.DietaryPreferences);
            existingEmployeeAllergiesAndDietaryPreferences.Comment = allergiesAndDietaryPreferences.Comment;
        }

        await _db.SaveChangesAsync();
    }

    public async Task Delete(EmployeeAllergiesAndDietaryPreferencesEntity employeeAllergy)
    {
        _db.Remove(employeeAllergy);
        await _db.SaveChangesAsync();
    }
}