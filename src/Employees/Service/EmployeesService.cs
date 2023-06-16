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
        var employee = await _employeesRepository.GetEmployeeAsync(alias, country);

        return employee;
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

    public async Task<EmployeeInformation?> GetInformationByEmployee(EmployeeEntity employee)
    {
        var employeeInformation = await _employeesRepository.GetEmployeeInformationAsync(employee);

        return employeeInformation == null ? null : ModelConverters.ToEmployeeInformation(employeeInformation);
    }


    public Task AddOrUpdateEmployeeInformation(EmployeeEntity employee, EmployeeInformation employeeInformation)
    {
        {
            EmployeeInformationEntity entity = new EmployeeInformationEntity
            {
                Employee = employee,
                Phone = employeeInformation.Phone,
                AccountNr = employeeInformation.AccountNr,
                Adress = employeeInformation.Adress,
                ZipCode = employeeInformation.ZipCode,
                City = employeeInformation.City,
            };

            return _employeesRepository.AddToDatabase(entity);
        }
    }

    public async Task<EmergencyContact?> GetEmergencyContactByEmployee(EmployeeEntity employee)
    {
        var emergencyContact = await _employeesRepository.GetEmergencyContactAsync(employee);

        return emergencyContact == null ? null : ModelConverters.ToEmergencyContact(emergencyContact);
    }

    public Task AddOrUpdateEmergencyContact(EmployeeEntity employee, EmergencyContact employeeInformation)
    {
        {
            EmergencyContactEntity entity = new EmergencyContactEntity
            {
                Employee = employee,
                Name = employeeInformation.Name,
                Phone = employeeInformation.Phone,
                Relation = employeeInformation.Relation,
                Comment = employeeInformation.Comment,
            };

            return _employeesRepository.AddToDatabase(entity);
        }
    }
}