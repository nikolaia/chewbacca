using Employees.Models;
using Employees.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employees.Service;

public class EmployeesService {

    private readonly EmployeeContext _db;

    public EmployeesService(EmployeeContext _db)
    {
        this._db = _db;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployees() 
    {
        // Henter alle Employees fra EmployeeContext

        // [optional] slå sammen data fra flere kilder
        // [optional] formater data på noe spesielt vis
        var lol = await _db.Employees.ToListAsync();
        return  lol;
    }

    public async Task AddOrUpdateEmployees(Employee employee) {
        _db.Add(employee);
        _db.SaveChanges();
        // TODO Async
    }
}