using ApplicationCore.Models;

namespace Web.ViewModels;

public static class ModelConverters
{
    public static EmployeeJson ToEmployeeJson(Employee employee)
    {
        return new EmployeeJson
        {
            Name = employee.EmployeeInformation.Name,
            Email = employee.EmployeeInformation.Email,
            Telephone = employee.EmployeeInformation.Telephone,
            ImageUrl = employee.EmployeeInformation.ImageUrl,
            OfficeName = employee.EmployeeInformation.OfficeName,
            StartDate = employee.EmployeeInformation.StartDate,
        };
    }

    public static EmployeeExtendedJson ToEmployeeExtendedJson(Employee employee)
    {
        return new EmployeeExtendedJson
        {
            Name = employee.EmployeeInformation.Name,
            Email = employee.EmployeeInformation.Email,
            Telephone = employee.EmployeeInformation.Telephone,
            ImageUrl = employee.EmployeeInformation.ImageUrl,
            OfficeName = employee.EmployeeInformation.OfficeName,
            StartDate = employee.EmployeeInformation.StartDate,
            AccountNumber = employee.EmployeeInformation.AccountNumber,
            Address = employee.EmployeeInformation.Address,
            ZipCode = employee.EmployeeInformation.ZipCode,
            City = employee.EmployeeInformation.City,
            EmergencyContact = employee.EmergencyContact,
            AllergiesAndDietaryPreferences = employee.EmployeeAllergiesAndDietaryPreferences != null
                ? ToEmployeeAllergiesAndDietaryPreferencesJson(employee.EmployeeAllergiesAndDietaryPreferences)
                : null
        };
    }

    public static EmployeeAllergiesAndDietaryPreferencesJson ToEmployeeAllergiesAndDietaryPreferencesJson(
        EmployeeAllergiesAndDietaryPreferences entity)
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
    
}