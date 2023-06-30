using Employees.Repositories;

namespace IntegrationTests;

public static class Utilities
{
    public static void InitializeDbForTests(EmployeeContext db)
    {
        db.Employees.AddRange(Seed.GetSeedingEmployees());
        db.SaveChanges();

        var employee = db.Employees.FirstOrDefault(e => e.Email == "test@variant.no")!;

        db.EmergencyContacts.AddRange(Seed.GetSeedingEmergencyContact(employee));

        db.SaveChanges();
    }
}