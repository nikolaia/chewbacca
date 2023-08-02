
using Employees.Models;

using Microsoft.EntityFrameworkCore;

namespace Employees.Repositories;

public class EmergencyContactRepository
{
    private readonly EmployeeContext _db;
    private readonly EmployeesRepository _employeesRepository;

    public EmergencyContactRepository(EmployeeContext db, EmployeesRepository employeesRepository)
    {
        _db = db;
        _employeesRepository = employeesRepository;
    }

    public async Task<EmergencyContactEntity?> GetByEmployee(EmployeeEntity employee)
    {
        return await _db.EmergencyContacts
                    .Where(emp => emp.Employee.Equals(employee))
                    .SingleOrDefaultAsync();
    }

    public async Task<bool> AddOrUpdateEmergencyContact(string alias, string country,
        EmergencyContact emergencyContact)
    {
        var employee = await _employeesRepository.GetEmployeeAsync(alias, country);

        if (employee == null)
        {
            return false;
        }

        employee.EmergencyContact = new EmergencyContactEntity
        {
            Employee = employee,
            Name = emergencyContact.Name,
            Phone = emergencyContact.Phone,
            Relation = emergencyContact.Relation,
            Comment = emergencyContact.Comment
        };
        var changes = await _db.SaveChangesAsync();
        return changes > 0;
    }
}