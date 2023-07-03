using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Employees.Models;

[Index(nameof(Email), IsUnique = true)]
public record EmployeeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    [StringLength(maximumLength: 100)]
    public string Email { get; set; } = null!;

    public string? Telephone { get; set; }
    public string? ImageUrl { get; set; }
    public string OfficeName { get; set; } = null!;

    [StringLength(maximumLength: 3)]
    public string CountryCode { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string? AccountNumber { get; set; }
    public string? Address { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }

    public EmployeeAllergiesAndDietaryPreferencesEntity? AllergiesAndDietaryPreferences { get; set; }
    public EmergencyContactEntity? EmergencyContact { get; set; }
}