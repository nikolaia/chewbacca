using Employees.Models;
using Employees.Repositories;

namespace Employees.Service;

public class EmployeesService
{
    private readonly EmployeesRepository _employeesRepository;

    public EmployeesService(EmployeesRepository employeesRepository)
    {
        this._employeesRepository = employeesRepository;
    }

    private static bool IsEmployeeActive(EmployeeEntity employee)
    {
        return DateTime.Now > employee.StartDate &&
               (employee.EndDate == null || DateTime.Now < employee.EndDate);
    }

    /**
     * <returns>list of employees from database</returns>
     */
    public async Task<IEnumerable<Employee>> GetActiveEmployees(string? country = null)
    {
        var employees = await (string.IsNullOrEmpty(country)
            ? _employeesRepository.GetAllEmployees()
            : _employeesRepository.GetEmployeesByCountry(country));

        return employees
            .Where(IsEmployeeActive)
            .Select(ModelConverters.ToEmployee);
    }

    public async Task<Employee?> GetByAliasAndCountry(string alias, string country)
    {
        var employee = await _employeesRepository.GetEmployeeAsync(alias, country);
        return employee == null ? null : ModelConverters.ToEmployee(employee);
    }

    public async Task<EmployeeEntity?> GetEntityByAliasAndCountry(string alias, string country)
    {
        return await _employeesRepository.GetEmployeeAsync(alias, country);
    }

    public Task AddOrUpdateEmployee(EmployeeEntity employee)
    {
        return _employeesRepository.AddToDatabase(employee);
    }

    public Task<string?> EnsureEmployeeIsDeleted(string email)
    {
        return _employeesRepository.EnsureEmployeeIsDeleted(email);
    }

    public Task<IEnumerable<string?>> EnsureEmployeesWithEndDateBeforeTodayAreDeleted()
    {
        return _employeesRepository.EnsureEmployeesWithEndDateBeforeTodayAreDeleted();
    }

    public Task UpdateEmployeeInformation(EmployeeEntity employee, EmployeeInformation employeeInformation)
    {
        employee.Telephone = employeeInformation.Phone;
        employee.Address = employeeInformation.Address;
        employee.AccountNumber = employeeInformation.AccountNumber;
        employee.ZipCode = employeeInformation.ZipCode;
        employee.City = employeeInformation.City;

        return _employeesRepository.AddToDatabase(employee);
    }

    public async Task<EmergencyContact?> GetEmergencyContactByEmployee(EmployeeEntity employee)
    {
        var emergencyContact = await _employeesRepository.GetEmergencyContactAsync(employee);

        return emergencyContact == null ? null : ModelConverters.ToEmergencyContact(emergencyContact);
    }

    public Task AddOrUpdateEmergencyContact(EmployeeEntity employee, EmergencyContact emergencyContact)
    {
        var entity = new EmergencyContactEntity
        {
            Employee = employee,
            Name = emergencyContact.Name,
            Phone = emergencyContact.Phone,
            Relation = emergencyContact.Relation == "" ? null : emergencyContact.Relation,
            Comment = emergencyContact.Comment == "" ? null : emergencyContact.Comment,
        };

        return _employeesRepository.AddToDatabase(entity);
    }
    public Task AddOrUpdatePresentation(PresentationEntity presentation, Guid employeeId)
    {
        var entity = presentation with { EmployeeId = employeeId, };
        return _employeesRepository.AddToDatabase(entity);
    }

    public Boolean isValid(EmergencyContact emergencyContact)
    {
        return emergencyContact.Name.Length >= 2 && emergencyContact.Phone.Length >= 8;
    }
}