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
    public string Name { get; set; }
    [StringLength(maximumLength: 100)]
    public string Email { get; set; }
    public string? Telephone { get; set; }
    public string? ImageUrl { get; set; }
    public string OfficeName { get; set; }
    public DateTime StartDate { get; set; }
}