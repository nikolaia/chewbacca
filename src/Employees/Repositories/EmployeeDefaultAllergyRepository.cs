using Employees.Models;

using Microsoft.EntityFrameworkCore;

namespace Employees.Repositories;

public class EmployeeDefaultAllergiesRepository
{
    private readonly EmployeeContext _db;

    public EmployeeDefaultAllergiesRepository(EmployeeContext db)
    {
        _db = db;
    }

    private async Task<EmployeeDefaultAllergyEntity?> GetAsync(EmployeeEntity employee, DefaultAllergyEnum allergy)
    {
        return await _db.EmployeeDefaultAllergies
            .Where(e => e.Employee.Equals(employee))
            .Where(e => e.DefaultAllergy.Equals(allergy))
            .SingleOrDefaultAsync();
    }

    public async Task<List<EmployeeDefaultAllergyEntity>> GetByEmployee(EmployeeEntity employee)
    {
        return await _db.EmployeeDefaultAllergies
            .Where(e => e.Employee.Equals(employee))
            .ToListAsync();
    }

    public async Task AddToDatabase(EmployeeEntity employee, DefaultAllergyEnum allergy)
    {
        EmployeeDefaultAllergyEntity? existingEmployeeDefaultAllergies = await GetAsync(employee, allergy);

        if (existingEmployeeDefaultAllergies == null)
        {
            var employeeAllergy = new EmployeeDefaultAllergyEntity
            {
                Employee = employee,
                DefaultAllergy = allergy
            };

            _db.Add(employeeAllergy);
        }

        _db.SaveChanges();
    }

    public async Task Delete(EmployeeDefaultAllergyEntity employeeAllergy)
    {
        _db.Remove(employeeAllergy);
        await _db.SaveChangesAsync();
    }
}