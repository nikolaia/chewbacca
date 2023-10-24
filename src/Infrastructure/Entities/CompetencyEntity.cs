using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[PrimaryKey(nameof(Name), nameof(ProjectExperienceId))]
public class CompetencyEntity
{

    public required string Name { get; init; }
    public string ProjectExperienceId { get; set; } = null!;
    public ProjectExperienceEntity ProjectExperience { get; set; } = null!;
    public DateTime LastSynced { get; set; }
}