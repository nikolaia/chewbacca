using Database.Seed.Entities;

namespace Database.Seed;

public static class Seed
{
    public static List<Employee> GetSeedingEmployees()
    {
        return new List<Employee>()
        {
            new()
            {
                Email = "test@example.com",
                Name = "Navn",
                Telephone = "123982131",
                FullName = "Navn Navnersen",
                ImageUrl = "https://example.com/image.png",
                OfficeName = "Oslo"
            }
        };
    }
}