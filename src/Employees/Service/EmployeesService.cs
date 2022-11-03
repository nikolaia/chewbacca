using Employees.Models;
using Employees.Repositories;

namespace Employees.Service;

public class EmployeesService {

    private readonly EmployeesRepository _employeesRepository;

    public EmployeesService(EmployeesRepository _employeesRepository)
    {
        this._employeesRepository = _employeesRepository;
    }

    /**
     * <returns>list of employees from database</returns>
     */
    public async Task<IEnumerable<Employee>> GetAllEmployees() 
    {
        return await _employeesRepository.GetAllEmployees();
    }

    public async Task AddOrUpdateEmployees(EmployeeEntity employee)
    {
        await _employeesRepository.AddToDatabase(employee);
    }
}