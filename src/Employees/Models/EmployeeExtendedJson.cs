namespace Employees.Models;

public record EmployeeExtendedJson
{
    public string Email { get; init; } = null!;
    public string Name { get; set; } = null!;
    public string? Telephone { get; set; }
    public string? ImageUrl { get; set; }
    public string OfficeName { get; set; } = null!;
    public DateTime StartDate { get; set; }

    public EmployeeInformation? Information { get; set; } = null!;
    public EmergencyContact? EmergencyContact { get; set; } = null!;
    public string? AccountNumber { get; init; }
    public string? Address { get; init; }
    public string? ZipCode { get; init; }
    public string? City { get; init; }
    public AllergiesAndDietaryPreferences AllergiesAndDietaryPreferences { get; set; } = null!;
}