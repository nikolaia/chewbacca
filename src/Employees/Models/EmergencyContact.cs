using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Employees.Models;

public record EmergencyContact
{
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Relation { get; set; } = null!;
    public string Comment { get; set; } = null!;
}