using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employees.Models;

public enum DietaeryPreferenceEnum
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

    public DietaeryPreferenceEnum DietaryPreference { get; set; }
}