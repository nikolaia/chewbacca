using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public record ProjectExperienceEntity
{
    [Key] 
    public string? Id { get; set; }

    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;
    public string? MonthFrom { get; set; }
    public string? YearFrom { get; set; }
    public string? MonthTo { get; set; }
    public string? YearTo { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime LastSynced { get; set; }
    public Uri? Url { get; set; }
}