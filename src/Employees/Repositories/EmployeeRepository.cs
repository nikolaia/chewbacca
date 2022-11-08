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
        var tmp = _db.Employees.SingleOrDefault(e => e.Email == employee.Email);
        if (tmp != null)
        {
            tmp.Email = employee.Email;
            tmp.Name = employee.Name;
            tmp.ImageUrl = employee.ImageUrl;
            tmp.FullName = employee.FullName;
            tmp.Telephone = employee.Telephone;
            tmp.OfficeName = employee.OfficeName;
        }
        else
        {
            _db.Add(employee);
        }
        await _db.SaveChangesAsync();
    }
}