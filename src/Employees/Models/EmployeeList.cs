namespace Employees.Models;

public record EmployeeList
{
    public IEnumerable<Employee> Employees { get; init; }
};