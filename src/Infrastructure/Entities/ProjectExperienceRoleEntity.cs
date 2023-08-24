using System.ComponentModel.DataAnnotations;

using ApplicationCore.Models;

namespace Infrastructure.Entities;

public record ProjectExperienceRoleEntity
{
    [Key] public string? Id { get; set; }
    public string Title { get; set; }

    public string Description { get; set; }
    
    public string ProjectExperienceId { get; set; }
    
    public ProjectExperienceEntity ProjectExperience { get; set; } = null!;

    public DateTime LastSynced { get; set; }
}

public static class ProjectExperienceRoleExtension
{
    public static ProjectExperienceRole ToProjectExperienceRole(ProjectExperienceRoleEntity entity)
    {
        return new ProjectExperienceRole { Description = entity.Description, Title = entity.Title, Id = entity.Id };
    }
}