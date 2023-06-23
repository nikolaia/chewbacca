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

    private async Task<EmployeeDietaryPreferenceEntity?> GetAsync(EmployeeEntity employee, DietaeryPreferenceEnum dietaryPreference)
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

    public async Task AddToDatabase(EmployeeDietaryPreferenceEntity employeeDietaryPreferenceEntity)
    {
        EmployeeDietaryPreferenceEntity? existingEmployeeDietaryPreference = await GetAsync(employeeDietaryPreferenceEntity.Employee, employeeDietaryPreferenceEntity.DietaryPreference);

        if (existingEmployeeDietaryPreference == null)
        {
            _db.Add(employeeDietaryPreferenceEntity);
        }

        _db.SaveChanges();
    }

    public async Task Delete(EmployeeDietaryPreferenceEntity employeeDietaryPreferenceEntity)
    {
        _db.Remove(employeeDietaryPreferenceEntity);
        await _db.SaveChangesAsync();
    }
}