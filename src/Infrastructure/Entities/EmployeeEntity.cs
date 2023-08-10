﻿using System.ComponentModel.DataAnnotations;
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

    [StringLength(maximumLength: 100)]
    public string Email { get; set; } = null!;

    public string? Telephone { get; set; }
    public string? ImageUrl { get; set; }
    public string OfficeName { get; set; } = null!;

    [StringLength(maximumLength: 3)]
    public string CountryCode { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string? AccountNumber { get; set; }
    public string? Address { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }

    public EmergencyContactEntity? EmergencyContact { get; set; }
    public EmployeeAllergiesAndDietaryPreferencesEntity? AllergiesAndDietaryPreferences { get; set; }
}

public static class EmployeeEntityExtensions
{
       public static Employee ToEmployee(this EmployeeEntity employeeEntity)
        {
            return new Employee
            {
                Name = employeeEntity.Name,
                Email = employeeEntity.Email,
                Telephone = employeeEntity.Telephone,
                ImageUrl = employeeEntity.ImageUrl,
                OfficeName = employeeEntity.OfficeName,
                StartDate = employeeEntity.StartDate,
                EndDate = employeeEntity.EndDate,
                Address = employeeEntity.Address,
                ZipCode = employeeEntity.ZipCode,
                City = employeeEntity.City,
                AccountNumber = employeeEntity.AccountNumber
            };
        }
}