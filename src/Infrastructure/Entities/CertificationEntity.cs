using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class CertificationEntity
{
    [Key] public string Id { get; set; } = null!;

    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;
    public DateTime? ExpiryDate { get; set; }
    public string? IssuedMonth { get; set; }
    public string? IssuedYear { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime LastSynced { get; set; }
}