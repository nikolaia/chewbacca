using System.ComponentModel.DataAnnotations;

using ApplicationCore.Models;

namespace Infrastructure.Entities;

public record ProjectExperienceRoleEntity
{
    [Key] public required string Id { get; set; }
    public required string Title { get; set; }

    public required string Description { get; set; }
    
    public string ProjectExperienceId { get; set; } = null!;
    
    public ProjectExperienceEntity ProjectExperience { get; set; } = null!;

    public required DateTime LastSynced { get; set; }
}

public static class ProjectExperienceRoleEntityExtension
{
    public static ProjectExperienceRole ToProjectExperience(this ProjectExperienceRoleEntity pe)
    {
        return new ProjectExperienceRole
        {
            Description = pe.Description, Id = pe.Id, Title = pe.Title
        };
    }
}