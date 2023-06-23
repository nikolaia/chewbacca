using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employees.Models;

public enum DietaryPreferenceEnum
{
    KOSHER,
    NOT_BEEF,
    NOT_FISH_OR_SHELLFISH,
    HALAL,
    PESCETARIANS,
    VEGANS,
    VEGETARIANS,
    NO_PREFERENCES
}

public record EmployeeDietaryPreferenceEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public EmployeeEntity Employee { get; set; } = null!;

    public DietaryPreferenceEnum DietaryPreference { get; set; }
}