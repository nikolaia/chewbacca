using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employees.Models;

public enum DefaultAllergyEnum
{
    MILK,
    GLUTEN,
    EGG,
    FISH,
    PEANUTS
}

public enum DietaryPreferenceEnum
{
    KOSHER,
    NOT_BEEF,
    NOT_FISH_OR_SHELLFISH,
    HALAL,
    PESCETARIAN,
    VEGAN,
    VEGETARIAN,
    NO_PREFERENCES
}

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