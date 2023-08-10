using ApplicationCore.Interfaces;
using ApplicationCore.Models;

using Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EmployeeAllergiesAndDietaryPreferencesRepository : IEmployeeAllergiesAndDietaryPreferencesRepository
{
    private readonly EmployeeContext _db;
    private readonly EmployeesRepository _employeesRepository;

    public EmployeeAllergiesAndDietaryPreferencesRepository(EmployeeContext db, EmployeesRepository employeesRepository)
    {
        _db = db;
        _employeesRepository = employeesRepository;
    }

    public async Task<EmployeeAllergiesAndDietaryPreferences?> GetByEmployee(string alias, string country)
    {
        var employeeAllergiesAndDietaryPreferencesEntity = await _db.EmployeeAllergiesAndDietaryPreferences
            .Where(emp => emp.Employee.Email.StartsWith($"{alias}@"))
            .Where(emp => emp.Employee.CountryCode == country)
            .SingleOrDefaultAsync();
        return employeeAllergiesAndDietaryPreferencesEntity?.ToAllergiesAndDietaryPreferences();
    }

    public async Task<bool> AddOrUpdateEmployeeAllergiesAndDietaryPreferences(string alias, string country,
        EmployeeAllergiesAndDietaryPreferences allergiesAndDietaryPreferences)
    {
        var employee = await _employeesRepository.GetEmployeeEntity(alias, country);

        if (employee == null)
        {
            return false;
        }

        employee.AllergiesAndDietaryPreferences = new EmployeeAllergiesAndDietaryPreferencesEntity
        {
            Employee = employee,
            DefaultAllergies = allergiesAndDietaryPreferences.DefaultAllergies,
            OtherAllergies = allergiesAndDietaryPreferences.OtherAllergies,
            DietaryPreferences = allergiesAndDietaryPreferences.DietaryPreferences,
            Comment = allergiesAndDietaryPreferences.Comment
        };

        var changes = await _db.SaveChangesAsync();
        return changes > 0;
    }
}