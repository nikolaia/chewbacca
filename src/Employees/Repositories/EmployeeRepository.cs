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
            Id = employee.Id
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
            Id = employeeEntity.Id
        };
    }

    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        return await _db.Employees.Select(entity => ToEmployee(entity)).ToListAsync();
    }

    public async void AddToDatabase(Employee employee)
    {
        await _db.AddAsync(employee);
        await _db.SaveChangesAsync();
    }
}