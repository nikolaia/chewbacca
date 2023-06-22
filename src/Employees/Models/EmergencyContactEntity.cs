using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Employees.Models;

public record EmergencyContactEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public EmployeeEntity Employee { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Relation { get; set; }
    public string? Comment { get; set; }
}