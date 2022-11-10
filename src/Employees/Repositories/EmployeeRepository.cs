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
    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        var employees = await _db.Employees.ToListAsync();
        return employees.Select(ModelConverters.ToEmployee);
    }

    public async Task AddToDatabase(EmployeeEntity employee)
    {
        EmployeeEntity? updateEmployee = await _db.Employees.SingleOrDefaultAsync(e => e.Email == employee.Email);

        if (updateEmployee != null)
        {
            updateEmployee.Email = employee.Email;
            updateEmployee.Name = employee.Name;
            updateEmployee.ImageUrl = employee.ImageUrl;
            updateEmployee.FullName = employee.FullName;
            updateEmployee.Telephone = employee.Telephone;
            updateEmployee.OfficeName = employee.OfficeName;
        }
        else
        {
            await _db.AddAsync(employee);
        }
        await _db.SaveChangesAsync();
    }
}