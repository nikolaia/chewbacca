using Employees.Models;

namespace Employees.Repositories;

public static class Seed
{
    public static List<EmployeeEntity> GetSeedingEmployees()
    {
        return new List<EmployeeEntity>()
        {
            new()
            {
                Email = "test@variant.no",
                Name = "Navn",
                Telephone = "81529332",
                ImageUrl = "https://example.com/image.png",
                CountryCode = "no",
                OfficeName = "Oslo",
                StartDate = new DateTime(2018, 8, 1),
                EndDate = null,
                AccountNumber = null,
                Address = null,
                ZipCode = null,
                City = null,
                EmergencyContact = new EmergencyContactEntity()
                {
                    Name = "Ola Nordmann",
                    Phone = "12345678",
                    Relation = "Far",
                    Comment = "Jobber nattevakter, send melding først"
                },
                AllergiesAndDietaryPreferences = new EmployeeAllergiesAndDietaryPreferencesEntity()
                {
                    DefaultAllergies = new List<DefaultAllergyEnum> { DefaultAllergyEnum.MILK, DefaultAllergyEnum.EGG },
                    OtherAllergies = new List<string> { "Druer", "Pære" },
                    DietaryPreferences = new List<DietaryPreferenceEnum> { DietaryPreferenceEnum.VEGETARIAN },
                    Comment = "Reagerer bare på eggehvite, ikke eggeplomme",
                }
            },
            new()
            {
                Email = "test@variant.se",
                Name = "Navn",
                Telephone = "81529332",
                ImageUrl = "https://example.com/image.png",
                CountryCode = "se",
                OfficeName = "Oslo",
                StartDate = new DateTime(2018, 8, 1),
                EndDate = null,
                AccountNumber = null,
                Address = null,
                ZipCode = null,
                City = null
            }
        };
    }
}