using Employees.Models;

using Microsoft.EntityFrameworkCore;

namespace Employees.Repositories;

public class EmployeeOtherAllergiesRepository
{
    private readonly EmployeeContext _db;

    public EmployeeOtherAllergiesRepository(EmployeeContext db)
    {
        _db = db;
    }

    private async Task<EmployeeOtherAllergyEntity?> GetAsync(EmployeeOtherAllergyEntity allergy)
    {
        return await _db.EmployeeOtherAllergies
            .Where(e => e.Employee.Equals(allergy.Employee))
            .Where(e => e.OtherAllergy != null && e.OtherAllergy.Equals(allergy.OtherAllergy))
            .SingleOrDefaultAsync();
    }

    private async Task<EmployeeOtherAllergyEntity?> GetAsync(EmployeeEntity employee, string otherAllergy)
    {
        return await _db.EmployeeOtherAllergies
            .Where(e => e.Employee.Equals(employee))
            .Where(e => e.OtherAllergy.Equals(otherAllergy))
            .SingleOrDefaultAsync();
    }

    public async Task<List<EmployeeOtherAllergyEntity>> GetByEmployee(EmployeeEntity employee)
    {
        return await _db.EmployeeOtherAllergies
            .Where(e => e.Employee.Equals(employee))
            .ToListAsync();
    }

    public async Task AddToDatabase(EmployeeOtherAllergyEntity employeeAllergy)
    {
        EmployeeOtherAllergyEntity? existingEmployeeOtherAllergies = await GetAsync(employeeAllergy);

        if (existingEmployeeOtherAllergies == null)
        {
            _db.Add(employeeAllergy);
        }

        _db.SaveChanges();
    }

    public async Task DeleteByEmployeeAndAllergy(EmployeeEntity employee, string otherAllergy)
    {
        EmployeeOtherAllergyEntity? employeeAllergy = await GetAsync(employee, otherAllergy);

        if (employeeAllergy != null)
        {
            _db.Remove(employeeAllergy);
            _db.SaveChanges();
        }
    }
}