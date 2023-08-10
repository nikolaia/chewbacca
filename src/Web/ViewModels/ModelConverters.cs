using ApplicationCore.Models;

namespace Web.ViewModels;

public static class ModelConverters
{
    public static EmployeeJson ToEmployeeJson(Employee employee)
    {
        return new EmployeeJson
        {
            Name = employee.Name,
            Email = employee.Email,
            Telephone = employee.Telephone,
            ImageUrl = employee.ImageUrl,
            OfficeName = employee.OfficeName,
            StartDate = employee.StartDate,
        };
    }

    public static EmployeeExtendedJson ToEmployeeExtendedJson(Employee employee,
        EmergencyContact? emergencyContact, EmployeeAllergiesAndDietaryPreferences? allergiesAndDietaryPreferences)
    {
        return new EmployeeExtendedJson
        {
            Name = employee.Name,
            Email = employee.Email,
            Telephone = employee.Telephone,
            ImageUrl = employee.ImageUrl,
            OfficeName = employee.OfficeName,
            AccountNumber = employee.AccountNumber,
            Address = employee.Address,
            ZipCode = employee.ZipCode,
            City = employee.City,
            EmergencyContact = emergencyContact,
            AllergiesAndDietaryPreferences = ToEmployeeAllergiesAndDietaryPreferencesJson(allergiesAndDietaryPreferences)
        };
    }
    
    public static EmployeeAllergiesAndDietaryPreferencesJson ToEmployeeAllergiesAndDietaryPreferencesJson(EmployeeAllergiesAndDietaryPreferences entity)
    {
        return new EmployeeAllergiesAndDietaryPreferencesJson()
        {
            Comment = entity.Comment,
            DietaryPreferences = entity.DietaryPreferences.Select(e => e.ToString()).ToList(),
            DefaultAllergies = entity.DefaultAllergies.Select(e => e.ToString()).ToList(),
            OtherAllergies = entity.OtherAllergies.Select(e => e.ToString()).ToList()
        };
    }
    
    public static List<DefaultAllergyEnum> DefaultAllergyStringListToEnumList(List<string> allergies)
    {
        return allergies.ConvertAll(allergy => (DefaultAllergyEnum)Enum.Parse(typeof(DefaultAllergyEnum), allergy));
    }

    public static List<DietaryPreferenceEnum> DietaryPreferenceStringListToEnumList(List<string> dietaryPreferences)
    {
        return dietaryPreferences.ConvertAll(dieteryPreference =>
            (DietaryPreferenceEnum)Enum.Parse(typeof(DietaryPreferenceEnum), dieteryPreference));
    }
    
    public static Presentation ToPresentation(Presentation entity)
    {
        return new Presentation()
        {
            Title = entity.Title,
            Description = entity.Description,
            Month = entity.Month,
            Year = entity.Year
        };
    }

    public static WorkExperience ToWorkExperience(WorkExperience entity)
    {
        return new WorkExperience()
        {
            Title = entity.Title,
            Description = entity.Description,
            MonthFrom = entity.MonthFrom,
            MonthTo = entity.MonthTo,
            YearFrom = entity.YearFrom,
            YearTo = entity.YearTo
        };
    }

    public static ProjectExperience ToProjectExperience(ProjectExperience entity)
    {
        return new ProjectExperience()
        {
            Title = entity.Title,
            Description = entity.Description,
            MonthFrom = entity.MonthFrom,
            MonthTo = entity.MonthTo,
            YearFrom = entity.YearFrom,
            YearTo = entity.YearTo
        };
    }


}