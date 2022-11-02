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
        // [optional] slå sammen data fra flere kilder
        // [optional] formater data på noe spesielt vis
        return await _employeesRepository.GetAllEmployees();
    }

    public Task AddOrUpdateEmployees(Employee employee)
    {
        _employeesRepository.AddToDatabase(employee);
        return Task.CompletedTask;
    }
}