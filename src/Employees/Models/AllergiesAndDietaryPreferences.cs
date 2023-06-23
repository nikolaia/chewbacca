namespace Employees.Models;

public record AllergiesAndDietaryPreferences
{
    public List<string> DefaultAllergies { get; set; } = null!;
    public List<string> OtherAllergies { get; set; } = null!;
    public List<string> DietaryPreferences { get; set; } = null!;
    public string Comment { get; set; } = null!;
}