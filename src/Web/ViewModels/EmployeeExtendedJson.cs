using ApplicationCore.Models;

namespace Web.ViewModels;

public record EmployeeExtendedJson
{
    public string Email { get; init; } = null!;
    public string Name { get; set; } = null!;
    public string? Telephone { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageThumbUrl { get; set; }
    public string OfficeName { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public string? AccountNumber { get; init; }
    public string? Address { get; init; }
    public string? ZipCode { get; init; }
    public string? City { get; init; }
    public EmergencyContact? EmergencyContact { get; set; } = null!;
    public EmployeeAllergiesAndDietaryPreferencesJson? AllergiesAndDietaryPreferences { get; set; } = null!;
}