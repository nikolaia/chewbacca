using System.ComponentModel.DataAnnotations;

using ApplicationCore.Models;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

public record ProjectExperienceEntity
{
    [Key] public string Id { get; set; } = null!;

    public Guid EmployeeId { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;
    public string? MonthFrom { get; set; }
    public string? YearFrom { get; set; }
    public string? MonthTo { get; set; }
    public string? YearTo { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime LastSynced { get; set; }
    public Uri? Url { get; set; }
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
            MonthFrom = pe.MonthFrom,
            MonthTo = pe.MonthTo,
            YearFrom = pe.YearFrom,
            YearTo = pe.YearTo,
            Roles = pe.ProjectExperienceRoles.Select(pEntity => new ProjectExperienceRole
            {
                Description = pEntity.Description, Id = pEntity.Id, Title = pEntity.Title
            }).ToList(),
            Competencies = pe.Competencies.Select(entity => entity.Name).ToHashSet()
        };
    }
}