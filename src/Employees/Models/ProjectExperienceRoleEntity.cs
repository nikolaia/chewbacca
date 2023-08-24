using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Employees.Models;

public class ProjectExperienceRoleEntity
{
    [Key]
    public string? Id { get; set; }
    public string title { get; set; }

    public string projectId { get; set; }

    public string description { get; set; }

    public DateTime LastSynced { get; set; }
}