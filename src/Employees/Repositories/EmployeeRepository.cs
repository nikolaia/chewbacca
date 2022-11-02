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
        return await _db.Employees.ToListAsync();
    }

    public async void AddToDatabase(Employee employee)
    {
        await _db.AddAsync(employee);
        await _db.SaveChangesAsync();
    }
}