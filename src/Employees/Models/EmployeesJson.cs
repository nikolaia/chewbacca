namespace Employees.Models;

public record EmployeesJson
{
    public IEnumerable<EmployeeJson> Employees { get; init; } = null!;
};

public record EmployeeJson
{
    public string Email { get; init; } = null!;
    public string Name { get; set; } = null!;
    public string? Telephone { get; set; }
    public string? ImageUrl { get; set; }
    public string OfficeName { get; set; } = null!;
    public DateTime StartDate { get; set; }
}