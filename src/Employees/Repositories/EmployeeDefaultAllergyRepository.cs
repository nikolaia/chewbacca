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

    private async Task<EmployeeDefaultAllergyEntity?> GetAsync(EmployeeDefaultAllergyEntity allergy)
    {
        return await _db.EmployeeDefaultAllergies
            .Where(e => e.Employee.Equals(allergy.Employee))
            .Where(e => e.DefaultAllergy.Equals(allergy.DefaultAllergy))
            .SingleOrDefaultAsync();
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

    public async Task AddToDatabase(EmployeeDefaultAllergyEntity employeeAllergy)
    {
        EmployeeDefaultAllergyEntity? existingEmployeeDefaultAllergies = await GetAsync(employeeAllergy);

        if (existingEmployeeDefaultAllergies == null)
        {
            _db.Add(employeeAllergy);
        }

        _db.SaveChanges();
    }

    public async Task DeleteByEmployeeAndAllergy(EmployeeEntity employee, DefaultAllergyEnum allergy)
    {
        EmployeeDefaultAllergyEntity? employeeAllergy = await GetAsync(employee, allergy);

        if (employeeAllergy != null)
        {
            _db.Remove(employeeAllergy);
            _db.SaveChanges();
        }
    }
}