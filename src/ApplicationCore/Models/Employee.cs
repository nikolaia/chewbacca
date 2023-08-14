namespace ApplicationCore.Models;

public record Employee
{
    public EmployeeInformation? EmployeeInformation { get; set; }
    public EmployeeAllergiesAndDietaryPreferences? EmployeeAllergiesAndDietaryPreferences { get; set; }
    public EmergencyContact? EmergencyContact { get; set; }
    public Cv? Cv { get; set; }
}