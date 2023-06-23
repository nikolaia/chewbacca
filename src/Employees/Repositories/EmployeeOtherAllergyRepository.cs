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

    public async Task AddToDatabase(EmployeeEntity employee, string allergy)
    {
        EmployeeOtherAllergyEntity? existingEmployeeOtherAllergies = await GetAsync(employee, allergy);

        if (existingEmployeeOtherAllergies == null)
        {
            var employeeAllergy = new EmployeeOtherAllergyEntity
            {
                Employee = employee,
                OtherAllergy = allergy
            };

            _db.Add(employeeAllergy);
        }

        _db.SaveChanges();
    }

    public async Task Delete(EmployeeOtherAllergyEntity employeeAllergy)
    {
        if (employeeAllergy != null)
        {
            _db.Remove(employeeAllergy);
            await _db.SaveChangesAsync();
        }
    }
}