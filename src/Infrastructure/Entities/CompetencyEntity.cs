using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[PrimaryKey(nameof(Name), nameof(ProjectExperienceId))]
public class CompetencyEntity
{

    public string Name { get; init; } = null!;
    public string ProjectExperienceId { get; set; }
    
    public ProjectExperienceEntity ProjectExperience { get; set; } = null!;
    public DateTime LastSynced { get; set; }
}