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
    public async Task<IEnumerable<Employee>> GetAllEmployees() 
    {
        return await _employeesRepository.GetAllEmployees();
    }

    public async Task AddOrUpdateEmployee(EmployeeEntity employee)
    {
        await _employeesRepository.AddToDatabase(employee);
    }
}