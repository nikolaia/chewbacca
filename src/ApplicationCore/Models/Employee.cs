namespace ApplicationCore.Models;

public record Employee
{
    public required EmployeeInformation EmployeeInformation { get; init; }
    public EmployeeAllergiesAndDietaryPreferences? EmployeeAllergiesAndDietaryPreferences { get; init; }
    public EmergencyContact? EmergencyContact { get; init; }
}