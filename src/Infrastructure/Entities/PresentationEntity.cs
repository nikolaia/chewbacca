using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

// [Index(nameof(Email), IsUnique = true)]
public record PresentationEntity
{
    [Key] public string Id { get; set; } = null!;
    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;
    public string? Month { get; set; }
    public string? Year { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime LastSynced { get; set; }
    public Uri? Url { get; set; }
}