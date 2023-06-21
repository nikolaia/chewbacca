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

public record EmployeeDefaultAllergyEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public EmployeeEntity Employee { get; set; } = null!;

    public DefaultAllergyEnum DefaultAllergy { get; set; }
}