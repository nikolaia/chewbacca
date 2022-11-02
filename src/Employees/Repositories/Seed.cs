namespace Employees;

using Employees.Models;

public static class Seed
{
    public static List<EmployeeEntity> GetSeedingEmployees()
    {
        return new List<EmployeeEntity>()
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