using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

// [Index(nameof(Email), IsUnique = true)]
public record PresentationEntity
{
    [Key] public required string Id { get; set; }
    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;

    public DateOnly? Date {get; set;}
    public int Order { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public required DateTime LastSynced { get; set; }
    public Uri? Url { get; set; }
}