using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Employees.Models;

public record EmployeeInformationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public EmployeeEntity Employee { get; set; } = null!;

    public string Phone { get; set; } = null!;
    public string AccountNr { get; set; } = null!;
    public string Adress { get; set; } = null!;
    public int PostalCode { get; set; }
    public string City { get; set; } = null!;
}