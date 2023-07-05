using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Employees.Models;

// [Index(nameof(Email), IsUnique = true)]
public record PresentationEntity
{
    [Key]
    public string? Id { get; set; }
    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;
    public string Month { get; set; } = null!;
    public string Year { get; set; } = null!;
    public int Order { get; set; } = 0;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Uri? Url { get; set; }

}