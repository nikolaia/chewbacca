namespace ApplicationCore.Entities;

public enum DefaultAllergyEnum
{
    MILK,
    GLUTEN,
    EGG,
    FISH,
    PEANUTS
}

public enum DietaryPreferenceEnum
{
    KOSHER,
    NOT_BEEF,
    NOT_FISH_OR_SHELLFISH,
    HALAL,
    PESCETARIAN,
    VEGAN,
    VEGETARIAN,
    NO_PREFERENCES
}

public record EmployeeAllergiesAndDietaryPreferences
{
    public List<DefaultAllergyEnum> DefaultAllergies { get; set; } = null!;
    public List<string> OtherAllergies { get; set; } = null!;
    public List<DietaryPreferenceEnum> DietaryPreferences { get; set; } = null!;

    public string? Comment { get; set; }
}
