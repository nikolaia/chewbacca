using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

using Infrastructure.Models;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EmergencyContactRepository : IEmergencyContactRepository
{
    private readonly EmployeeContext _db;
    private readonly EmployeesRepository _employeesRepository;

    public EmergencyContactRepository(EmployeeContext db, EmployeesRepository employeesRepository)
    {
        _db = db;
        _employeesRepository = employeesRepository;
    }

    public async Task<EmergencyContact?> GetByEmployee(string alias, string country)
    {
        var emergencyContacts = await _db.EmergencyContacts
                    .Where(emp => emp.Employee.Email.StartsWith($"{alias}@"))
                    .Where(emp => emp.Employee.CountryCode == country)
                    .SingleOrDefaultAsync();
        return emergencyContacts.ToEmergencyContact();
    }

    public async Task<bool> AddOrUpdateEmergencyContact(string alias, string country,
        EmergencyContact emergencyContact)
    {
        var employee = await _employeesRepository.GetEmployeeEntity(alias, country);

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