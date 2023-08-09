using ApplicationCore.Entities;

namespace ApplicationCore.Services;

public class EmployeeAllergiesAndDietaryPreferencesService
{
    public static List<string> DefaultAllergyEnumListToStringList(List<DefaultAllergyEnum> allergies)
    {
        return allergies.Select(allergy => allergy.ToString()).ToList();
    }

    public static List<DefaultAllergyEnum> DefaultAllergyStringListToEnumList(List<string> allergies)
    {
        return allergies.ConvertAll(allergy => (DefaultAllergyEnum)Enum.Parse(typeof(DefaultAllergyEnum), allergy));
    }

    public static List<string> DietaryPreferenceEnumListToStringList(List<DietaryPreferenceEnum> dietaryPreferences)
    {
        return dietaryPreferences.Select(dietaryPreference => dietaryPreference.ToString()).ToList();
    }

    public static List<DietaryPreferenceEnum> DietaryPreferenceStringListToEnumList(List<string> dietaryPreferences)
    {
        return dietaryPreferences.ConvertAll(dieteryPreference =>
            (DietaryPreferenceEnum)Enum.Parse(typeof(DietaryPreferenceEnum), dieteryPreference));
    }
}