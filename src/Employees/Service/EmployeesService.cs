using Employees.Models;
using Employees.Repositories;

namespace Employees.Service;

public class EmployeesService {

    private readonly EmployeesRepository _employeesRepository;

    public EmployeesService(EmployeesRepository employeesRepository)
    {
        this._employeesRepository = employeesRepository;
    }

    /**
     * <returns>list of employees from database</returns>
     */
    public Task<IEnumerable<Employee>> GetAllEmployees() 
    {
        return _employeesRepository.GetAllEmployees();
    }

    public Task AddOrUpdateEmployee(EmployeeEntity employee)
    {
        return _employeesRepository.AddToDatabase(employee);
    }

    public async Task EnsureEmployeeIsDeleted(string email)
    {
        await _employeesRepository.EnsureEmployeeIsDeleted(email); 
    }
}