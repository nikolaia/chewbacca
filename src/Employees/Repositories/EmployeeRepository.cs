using Employees.Models;
using Microsoft.EntityFrameworkCore;

namespace Employees.Repositories;

public class EmployeesRepository
{
    private readonly EmployeeContext _db;

    public EmployeesRepository(EmployeeContext _db)
    {
        this._db = _db;
    }

    private static EmployeeEntity ToEntity(Employee employee)
    {
        return new EmployeeEntity
        {
            FullName = employee.FullName,
            Name = employee.Name,
            Email = employee.Email,
            Telephone = employee.Telephone,
            ImageUrl = employee.ImageUrl,
            OfficeName = employee.OfficeName,
        };
    }

    private static Employee ToEmployee(EmployeeEntity employeeEntity)
    {
        return new Employee
        {
            FullName = employeeEntity.FullName,
            Name = employeeEntity.Name,
            Email = employeeEntity.Email,
            Telephone = employeeEntity.Telephone,
            ImageUrl = employeeEntity.ImageUrl,
            OfficeName = employeeEntity.OfficeName,
        };
    }
    
    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        var employees = await _db.Employees.ToListAsync();
        return employees.Select(ToEmployee);
    }

    public async Task AddToDatabase(EmployeeEntity employee)
    {
        _db.AddAsync(employee);
        await _db.SaveChangesAsync();
    }
}