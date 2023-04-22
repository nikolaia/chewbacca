namespace Employees.Models;

public record EmployeesJson
{
    public IEnumerable<EmployeeJson> Employees { get; init; }
};

public record EmployeeJson
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string? Telephone { get; set; }
    public string? ImageUrl { get; set; }
    public string OfficeName { get; set; }
    public DateTime StartDate { get; set; }
}