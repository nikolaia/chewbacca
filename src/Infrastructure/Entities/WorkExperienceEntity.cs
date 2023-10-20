using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public record WorkExperienceEntity
{
    [Key] public required string Id { get; set; }

    public required Guid EmployeeId { get; set; }
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