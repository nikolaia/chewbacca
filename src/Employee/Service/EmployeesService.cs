using Employee.Models;
public class EmployeesService {

    private readonly EmployeeContext _db;

    public EmployeesService(EmployeeContext _db)
    {
        this._db = _db;
    }

    public Task<IEnumerable<Employee>> GetAllEmployees() 
    {
        // Henter alle Employees fra EomployeeContext

        // [optional] slå sammen data fra flere kilder
        // [optional] formater data på noe spesielt vis

        throw new NotImplementedException();    
    }

}