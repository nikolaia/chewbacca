using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ApplicationCore.Entities;

namespace Infrastructure.Models;

public record EmergencyContactEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Relation { get; set; }
    public string? Comment { get; set; }

}

public static class EmergencyContactEntityExtensions
{
    public static EmergencyContact ToEmergencyContact(this EmergencyContactEntity emergencyContact)
    {
        return new EmergencyContact
        {
            Name = emergencyContact.Name,
            Phone = emergencyContact.Phone,
            Relation = emergencyContact.Relation,
            Comment = emergencyContact.Comment
        };
    }
}