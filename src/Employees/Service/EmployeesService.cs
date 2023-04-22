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
    public async Task<IEnumerable<Employee>> GetAllActiveEmployees()
    {
        var employees = await _employeesRepository.GetAllEmployees();
        return employees
            .Where(IsEmployeeActive)
            .Select(ModelConverters.ToEmployee);
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
}