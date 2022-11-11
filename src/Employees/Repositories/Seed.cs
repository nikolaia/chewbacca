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
                Email = "test@example.com",
                Name = "Navn",
                Telephone = "123982131",
                ImageUrl = "https://example.com/image.png",
                OfficeName = "Oslo"
            }
        };
    }
}