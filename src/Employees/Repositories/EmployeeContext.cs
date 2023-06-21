using Employees.Models;

using Microsoft.EntityFrameworkCore;

namespace Employees.Repositories;

public class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<EmployeeEntity> Employees { get; set; } = null!;
    public DbSet<EmergencyContactEntity> EmergencyContacts { get; set; } = null!;
    public DbSet<EmployeeDefaultAllergyEntity> EmployeeDefaultAllergies { get; set; } = null!;
    public DbSet<EmployeeOtherAllergyEntity> EmployeeOtherAllergies { get; set; } = null!;
    public DbSet<EmployeeDietaryPreferenceEntity> EmployeeDietaryPreferences { get; set; } = null!;
}