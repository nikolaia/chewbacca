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
}

public static class ProjectExperienceEntityExtension
{
    public static ProjectExperience ToProjectExperience(ProjectExperienceEntity entity)
    {
        return new ProjectExperience()
        {
            Description = entity.Description,
            MonthFrom = entity.MonthFrom,
            MonthTo = entity.MonthTo,
            YearFrom = entity.YearFrom,
            YearTo = entity.YearTo,
            Title = entity.Title,
            Id = entity.Id,
            Roles = entity.ProjectExperienceRoles.ToList()
                .Select(ProjectExperienceRoleExtension.ToProjectExperienceRole).ToList()
        };
    }
}