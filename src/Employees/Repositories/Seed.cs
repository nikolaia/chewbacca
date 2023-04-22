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
                OfficeName = "Oslo",
                StartDate = new DateTime(2018, 8, 1),
                EndDate = null
            },
            new()
            {
                Email = "test@variant.se",
                Name = "Navn",
                Telephone = "81529332",
                ImageUrl = "https://example.com/image.png",
                OfficeName = "Oslo",
                StartDate = new DateTime(2018, 8, 1),
                EndDate = null
            }
        };
    }
}