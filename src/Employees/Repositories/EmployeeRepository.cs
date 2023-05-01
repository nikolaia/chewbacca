using Employees.Models;

using Microsoft.EntityFrameworkCore;

namespace Employees.Repositories;

public class EmployeesRepository
{
    private readonly EmployeeContext _db;

    public EmployeesRepository(EmployeeContext db)
    {
        _db = db;
    }

    public async Task<List<EmployeeEntity>> GetAllEmployees()
    {
        return await _db.Employees.ToListAsync();
    }

    public async Task<EmployeeEntity?> GetEmployeeAsync(string alias, string country)
    {
        return await _db.Employees.Where(emp => emp.Email == $"{alias}@variant.{country}").SingleOrDefaultAsync();
    }

    public async Task AddToDatabase(EmployeeEntity employee)
    {
        EmployeeEntity? updateEmployee = await _db.Employees.SingleOrDefaultAsync(e => e.Email == employee.Email);

        if (updateEmployee != null)
        {
            updateEmployee.Email = employee.Email;
            updateEmployee.Name = employee.Name;
            updateEmployee.ImageUrl = employee.ImageUrl;
            updateEmployee.Telephone = employee.Telephone;
            updateEmployee.OfficeName = employee.OfficeName;
            updateEmployee.StartDate = employee.StartDate;
            updateEmployee.EndDate = employee.EndDate;
            updateEmployee.CountryCode = employee.CountryCode;
        }
        else
        {
            await _db.AddAsync(employee);
        }

        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes the employee from the database, if they exist, and returns the image url to the employees image blob that needs to be cleaned up
    /// </summary>
    /// <param name="email">Email of the employee</param>
    /// <returns>The image url to the employees image blob that needs to be cleaned up</returns>
    public async Task<string?> EnsureEmployeeIsDeleted(string email)
    {
        EmployeeEntity? employee = await _db.Employees.SingleOrDefaultAsync(e => e.Email == email);

        if (employee == null)
        {
            return null;
        }

        _db.Remove(employee);
        await _db.SaveChangesAsync();

        return employee.ImageUrl;
    }

    public async Task<IEnumerable<string?>> EnsureEmployeesWithEndDateBeforeTodayAreDeleted()
    {
        var employees = await _db.Employees.Where(e => e.EndDate < DateTime.Now).ToListAsync();

        if (!employees.Any())
        {
            return Array.Empty<string>();
        }

        _db.RemoveRange(employees);
        await _db.SaveChangesAsync();

        return employees.Select(employee => employee.ImageUrl);

    }
}