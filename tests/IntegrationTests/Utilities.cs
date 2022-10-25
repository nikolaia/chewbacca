using Database.Seed;

using Employee.Repositories;

namespace IntegrationTests;

public static class Utilities
{
    public static void InitializeDbForTests(EmployeeContext db)
    {
        db.Employees.AddRange(Seed.GetSeedingEmployees());
        db.SaveChanges();
    }

    public static void ReinitializeDbForTests(EmployeeContext db)
    {
        db.Employees.RemoveRange(db.Employees);
        InitializeDbForTests(db);
    }


    

}