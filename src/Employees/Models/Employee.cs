namespace Employees.Models;

public record Employee
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Telephone { get; set; }
    public string? ImageUrl { get; set; }
    public string OfficeName { get; set; }
}