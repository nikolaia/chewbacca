using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ApplicationCore.Models;

namespace Infrastructure.Entities;

public record EmployeeAllergiesAndDietaryPreferencesEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;

    public List<DefaultAllergyEnum> DefaultAllergies { get; set; } = null!;
    public List<string> OtherAllergies { get; set; } = null!;
    public List<DietaryPreferenceEnum> DietaryPreferences { get; set; } = null!;

    public string? Comment { get; set; }
}

public static class EmployeeAllergiesAndDietaryPreferencesEntityExtensions
{
    public static EmployeeAllergiesAndDietaryPreferences ToAllergiesAndDietaryPreferences(
        this EmployeeAllergiesAndDietaryPreferencesEntity employeeAllergiesAndDietaryPreferences)
    {
        return new EmployeeAllergiesAndDietaryPreferences
        {
            DefaultAllergies =
                employeeAllergiesAndDietaryPreferences.DefaultAllergies,
            OtherAllergies = employeeAllergiesAndDietaryPreferences.OtherAllergies,
            DietaryPreferences =
                employeeAllergiesAndDietaryPreferences.DietaryPreferences,
            Comment = employeeAllergiesAndDietaryPreferences.Comment ?? String.Empty
        };
    }
}