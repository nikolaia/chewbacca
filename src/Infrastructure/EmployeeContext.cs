using ApplicationCore.Models;

using Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure;

public class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<EmployeeEntity> Employees { get; set; } = null!;
    public DbSet<PresentationEntity> Presentations { get; set; } = null!;
    public DbSet<EmergencyContactEntity> EmergencyContacts { get; set; } = null!;

    public DbSet<EmployeeAllergiesAndDietaryPreferencesEntity> EmployeeAllergiesAndDietaryPreferences { get; set; } =
        null!;

    public DbSet<WorkExperienceEntity> WorkExperiences { get; set; } = null!;
    
    public DbSet<ProjectExperienceEntity> ProjectExperiences { get; set; }
    
    public DbSet<ProjectExperienceRoleEntity> ProjectExperienceRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var defaultAllergyConverter = new ValueConverter<List<DefaultAllergyEnum>, string>(
            v => string.Join(",", v),
            v => v.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(Enum.Parse<DefaultAllergyEnum>)
                .ToList()
        );

        var defaultAllergyComparer = new ValueComparer<List<DefaultAllergyEnum>>(
            (c1, c2) => c1 == null && c2 == null ? true : c1 == null || c2 != null ? false : c1.SequenceEqual(c2),
            c => c.GetHashCode(),
            c => c.ToList()
        );

        var dietaryPreferencesConverter = new ValueConverter<List<DietaryPreferenceEnum>, string>(
            v => string.Join(",", v),
            v => v.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(Enum.Parse<DietaryPreferenceEnum>)
                .ToList()
        );

        var dietaryPreferencesComparer = new ValueComparer<List<DietaryPreferenceEnum>>(
            (c1, c2) => c1 == null && c2 == null ? true : c1 == null || c2 == null ? false : c1.SequenceEqual(c2),
            c => c.GetHashCode(),
            c => c.ToList()
        );

        var stringConverter = new ValueConverter<List<string>, string>(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
        );

        var stringComparer = new ValueComparer<List<string>>(
            (c1, c2) => c1 == null && c2 == null ? true : c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c.GetHashCode(),
            c => c.ToList()
        );

        modelBuilder.Entity<EmployeeAllergiesAndDietaryPreferencesEntity>()
            .Property(b => b.DefaultAllergies)
            .HasConversion(defaultAllergyConverter)
            .Metadata.SetValueComparer(defaultAllergyComparer);

        modelBuilder.Entity<EmployeeAllergiesAndDietaryPreferencesEntity>()
            .Property(b => b.OtherAllergies)
            .HasConversion(stringConverter)
            .Metadata.SetValueComparer(stringComparer);

        modelBuilder.Entity<EmployeeAllergiesAndDietaryPreferencesEntity>()
            .Property(b => b.DietaryPreferences)
            .HasConversion(dietaryPreferencesConverter)
            .Metadata.SetValueComparer(dietaryPreferencesComparer);

    }
}