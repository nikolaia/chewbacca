using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public record WorkExperienceEntity
{
    [Key] public required string Id { get; set; }

    public required Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;

    public DateOnly? FromDate {get; set;}

    public DateOnly ToDate {get; set;}
    public int Order { get; set; }
    public required string Title { get; set; } 
    public required string Description { get; set; }

    public required string Company {get; set;}
    public DateTime LastSynced { get; set; }
    public Uri? Url { get; set; }
}