using Employees.Repositories;

namespace IntegrationTests;

public static class Utilities
{
    public static void InitializeDbForTests(EmployeeContext db)
    {
        db.Employees.AddRange(Seed.GetSeedingEmployees());

        db.SaveChanges();
    }
}