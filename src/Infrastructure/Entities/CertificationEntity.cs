using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class CertificationEntity
{
    [Key] public required string Id { get; set; }

    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;
    public DateOnly? ExpiryDate { get; set; }
    public DateOnly? IssuedDate { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; } 

    public required string Issuer {get; set;}
    public DateTime LastSynced { get; set; }
}