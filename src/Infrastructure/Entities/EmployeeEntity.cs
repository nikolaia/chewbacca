using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ApplicationCore.Models;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[Index(nameof(Email), IsUnique = true)]
public record EmployeeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    [StringLength(maximumLength: 100)] public string Email { get; set; } = null!;

    public string? Telephone { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageThumbUrl { get; set; }
    public string OfficeName { get; set; } = null!;

    [StringLength(maximumLength: 3)] public string CountryCode { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string? AccountNumber { get; set; }
    public string? Address { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }

    public EmergencyContactEntity? EmergencyContact { get; set; }
    public EmployeeAllergiesAndDietaryPreferencesEntity? AllergiesAndDietaryPreferences { get; set; }

    public List<ProjectExperienceEntity> ProjectExperiences { get; set; } = new();
    public List<WorkExperienceEntity> WorkExperiences { get; set; } = new();
    public List<PresentationEntity> Presentations { get; set; } = new();

    public List<CertificationEntity> Certifications { get; set; } = new();
}

public static class EmployeeEntityExtensions
{
    public static Employee ToEmployee(this EmployeeEntity employeeEntity)
    {
        return new Employee
        {
            EmployeeInformation = new EmployeeInformation
            {
                Name = employeeEntity.Name,
                Address = employeeEntity.Address,
                City = employeeEntity.City,
                Email = employeeEntity.Email,
                Telephone = employeeEntity.Telephone,
                AccountNumber = employeeEntity.AccountNumber,
                CountryCode = employeeEntity.CountryCode,
                EndDate = employeeEntity.EndDate,
                ImageUrl = employeeEntity.ImageUrl,
                ImageThumbUrl = employeeEntity.ImageThumbUrl,
                OfficeName = employeeEntity.OfficeName,
                StartDate = employeeEntity.StartDate,
                ZipCode = employeeEntity.ZipCode
            },
            EmergencyContact = employeeEntity.EmergencyContact != null
                ? new EmergencyContact
                {
                    Name = employeeEntity.EmergencyContact.Name,
                    Comment = employeeEntity.EmergencyContact.Comment,
                    Phone = employeeEntity.EmergencyContact.Phone,
                    Relation = employeeEntity.EmergencyContact.Relation
                }
                : null,
            EmployeeAllergiesAndDietaryPreferences = employeeEntity.AllergiesAndDietaryPreferences != null
                ? new EmployeeAllergiesAndDietaryPreferences
                {
                    Comment = employeeEntity.AllergiesAndDietaryPreferences.Comment,
                    DefaultAllergies = employeeEntity.AllergiesAndDietaryPreferences.DefaultAllergies,
                    DietaryPreferences = employeeEntity.AllergiesAndDietaryPreferences.DietaryPreferences,
                    OtherAllergies = employeeEntity.AllergiesAndDietaryPreferences.OtherAllergies
                }
                : null
        };
    }

    public static Cv ToCv(this EmployeeEntity employeeEntity)
    {
        return new Cv
        {
            Email = employeeEntity.Email,
            Presentations = employeeEntity.Presentations.Select(entity => new Presentation()
            {
                Description = entity.Description,
                Id = entity.Id,
                Title = entity.Title,
                Date = entity.Date
            }).ToList(),
            ProjectExperiences = employeeEntity.ProjectExperiences.Select(
                entity => entity.ToProjectExperience()).ToList(),
            WorkExperiences = employeeEntity.WorkExperiences.Select(entity => new WorkExperience
            {
                Description = entity.Description,
                Id = entity.Id,
                Title = entity.Title,
                FromDate = entity.FromDate,
                ToDate = entity.ToDate,
                Company = entity.Company
            }).ToList(),
            Certifiactions = employeeEntity.Certifications.Select(entity => new Certification
            {
                Description = entity.Description,
                Id = entity.Id,
                Title = entity.Title,
                IssuedDate = entity.IssuedDate,
                Issuer = entity.Issuer,
                ExpiryDate = entity.ExpiryDate
            }).ToList()
        };
    }
}