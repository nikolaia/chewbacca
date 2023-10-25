using System.ComponentModel.DataAnnotations;

using ApplicationCore.Models;

namespace Infrastructure.Entities;

public record ProjectExperienceEntity
{
    [Key] public required string Id { get; set; }

    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;

    public DateOnly? FromDate {get; set;}
    public DateOnly ToDate {get; set;}
    public int Order { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; } 

    public required string Customer {get; set;} 
    public required DateTime LastSynced { get; set; }
    public List<ProjectExperienceRoleEntity> ProjectExperienceRoles { get; set; } = new();

    public List<CompetencyEntity> Competencies { get; set; } = new();
}

public static class ProjectExperienceEntityExtension
{
    public static ProjectExperience ToProjectExperience(this ProjectExperienceEntity pe)
    {
        return new ProjectExperience
        {
            Id = pe.Id,
            Title = pe.Title,
            Description = pe.Description,
            ToDate = pe.ToDate,
            FromDate = pe.FromDate,
            Customer = pe.Customer,
            Roles = pe.ProjectExperienceRoles.Select(pEntity => new ProjectExperienceRole
            {
                Description = pEntity.Description, Id = pEntity.Id, Title = pEntity.Title
            }).ToList(),
            Competencies = pe.Competencies.Select(entity => entity.Name).ToHashSet()
        };
    }
}