using Employees.Models;

using Microsoft.EntityFrameworkCore;

namespace Employees.Repositories;

public class EmployeeDietaryPreferencesRepository
{
    private readonly EmployeeContext _db;

    public EmployeeDietaryPreferencesRepository(EmployeeContext db)
    {
        _db = db;
    }

    private async Task<EmployeeDietaryPreferenceEntity?> GetAsync(EmployeeEntity employee, DietaryPreferenceEnum dietaryPreference)
    {
        return await _db.EmployeeDietaryPreferences
            .Where(e => e.Employee.Equals(employee))
            .Where(e => e.DietaryPreference.Equals(dietaryPreference))
            .SingleOrDefaultAsync();
    }


    public async Task<List<EmployeeDietaryPreferenceEntity>> GetByEmployee(EmployeeEntity employee)
    {
        return await _db.EmployeeDietaryPreferences
            .Where(e => e.Employee.Equals(employee))
            .ToListAsync();
    }

    public async Task AddToDatabase(EmployeeEntity employee, DietaryPreferenceEnum dietaryPreference)
    {
        EmployeeDietaryPreferenceEntity? existingEmployeeDietaryPreference = await GetAsync(employee, dietaryPreference);

        if (existingEmployeeDietaryPreference == null)
        {
            var employeeDietaryPreference = new EmployeeDietaryPreferenceEntity
            {
                Employee = employee,
                DietaryPreference = dietaryPreference
            };

            _db.Add(employeeDietaryPreference);
        }

        _db.SaveChanges();
    }

    public async Task Delete(EmployeeDietaryPreferenceEntity employeeDietaryPreferenceEntity)
    {
        _db.Remove(employeeDietaryPreferenceEntity);
        await _db.SaveChangesAsync();
    }
}