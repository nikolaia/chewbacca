using ApplicationCore.Interfaces;
using ApplicationCore.Models;

using Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EmployeesRepository : IEmployeesRepository
{
    private readonly EmployeeContext _db;

    public EmployeesRepository(EmployeeContext db)
    {
        _db = db;
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        var employees = await _db.Employees.ToListAsync();
        return employees.Select(e => e.ToEmployee()).ToList();
    }

    public async Task<List<Employee>> GetEmployeesByCountry(string country)
    {
        var employees = await _db.Employees.Where(emp => emp.CountryCode == country).ToListAsync();
        return employees.Select(e => e.ToEmployee()).ToList();
    }

    internal async Task<EmployeeEntity?> GetEmployeeEntity(string alias, string country)
    {
        return await _db.Employees
            .Include(employee => employee.AllergiesAndDietaryPreferences)
            .Include(employee => employee.EmergencyContact)
            .Where(emp => emp.Email.StartsWith($"{alias}@"))
            .Where(emp => emp.CountryCode == country)
            .SingleOrDefaultAsync();
    }

    public async Task<Employee?> GetEmployeeAsync(string alias, string country)
    {
        var employee = await _db.Employees
            .Include(employee => employee.AllergiesAndDietaryPreferences)
            .Include(employee => employee.EmergencyContact)
            .Where(emp => emp.Email.StartsWith($"{alias}@"))
            .Where(emp => emp.CountryCode == country)
            .SingleOrDefaultAsync();
        return employee?.ToEmployee();
    }

    public async Task AddToDatabase(Employee employee)
    {
        EmployeeEntity? updateEmployee = await _db.Employees.SingleOrDefaultAsync(e => e.Email == employee.Email);

        if (updateEmployee != null)
        {
            updateEmployee.Email = employee.Email;
            updateEmployee.Name = employee.Name;
            updateEmployee.ImageUrl = employee.ImageUrl;
            updateEmployee.Telephone = employee.Telephone;
            updateEmployee.OfficeName = employee.OfficeName;
            updateEmployee.StartDate = employee.StartDate;
            updateEmployee.EndDate = employee.EndDate;
            // Don't set Address, AccountNumber, ZipCode and City since these aren't fetched from external sources,
            // and hence the information given from variantdash will be overwritten
        }
        else
        {
            await _db.AddAsync(new EmployeeEntity
            {
                Email = employee.Email,
                Name = employee.Name,
                ImageUrl = employee.ImageUrl,
                Telephone = employee.Telephone,
                OfficeName = employee.OfficeName,
                StartDate = employee.StartDate,
                EndDate = employee.EndDate,
                CountryCode = employee.CountryCode
            });
        }

        await _db.SaveChangesAsync();
    }

    public async Task<bool> UpdateEmployeeInformation(string alias, string country,
        EmployeeInformation employeeInformation)
    {
        var employee = await GetEmployeeEntity(alias, country);

        if (employee == null)
        {
            return false;
        }

        employee.Telephone = employeeInformation.Phone;
        employee.AccountNumber = employeeInformation.AccountNumber;
        employee.Address = employeeInformation.Address;
        employee.ZipCode = employeeInformation.ZipCode;
        employee.City = employeeInformation.City;

        var changes = await _db.SaveChangesAsync();
        return changes > 0;
    }

    /// <summary>
    /// Deletes the employee from the database, if they exist, and returns the image url to the employees image blob that needs to be cleaned up
    /// </summary>
    /// <param name="email">Email of the employee</param>
    /// <returns>The image url to the employees image blob that needs to be cleaned up</returns>
    public async Task<string?> EnsureEmployeeIsDeleted(string email)
    {
        EmployeeEntity? employee = await _db.Employees.SingleOrDefaultAsync(e => e.Email == email);

        if (employee == null)
        {
            return null;
        }

        _db.Remove(employee);
        await _db.SaveChangesAsync();

        return employee.ImageUrl;
    }

    public async Task<IEnumerable<string?>> EnsureEmployeesWithEndDateBeforeTodayAreDeleted()
    {
        var employees = await _db.Employees.Where(e => e.EndDate < DateTime.Now).ToListAsync();

        if (!employees.Any())
        {
            return Array.Empty<string>();
        }

        _db.RemoveRange(employees);
        await _db.SaveChangesAsync();

        return employees.Select(employee => employee.ImageUrl);
    }

    public async Task<EmergencyContact?> GetEmergencyContactAsync(Employee employee)
    {
        var emergencyContact = await _db.EmergencyContacts
            .Where(emp => emp.Employee.Email.Equals(employee.Email))
            .SingleOrDefaultAsync();
        return emergencyContact?.ToEmergencyContact();
    }

    public async Task AddToDatabase(string employeeEmail, EmergencyContact emergencyContact)
    {
        var updateEmergencyContact =
            await _db.EmergencyContacts.SingleOrDefaultAsync(e => e.Employee.Email == employeeEmail);

        if (updateEmergencyContact != null)
        {
            updateEmergencyContact.Name = emergencyContact.Name;
            updateEmergencyContact.Phone = emergencyContact.Phone;
            updateEmergencyContact.Relation = emergencyContact.Relation;
            updateEmergencyContact.Comment = emergencyContact.Comment;
        }
        else
        {
            await _db.AddAsync(emergencyContact);
        }

        await _db.SaveChangesAsync();
    }

    // public async Task AddToDatabase(string employeeEmail, List<Presentation> presentations)
    // {
    //     foreach (PresentationEntity presentationEntity in presentations)
    //     {
    //         await AddToDatabase(presentationEntity);
    //     }
    //
    //     await _db.SaveChangesAsync();
    // }
    //
    // public async Task AddToDatabase(List<WorkExperience> presentations)
    // {
    //     foreach (WorkExperienceEntity presentationEntity in presentations)
    //     {
    //         await AddToDatabase(presentationEntity);
    //     }
    //
    //     await _db.SaveChangesAsync();
    // }
    //
    // public async Task AddToDatabase(List<ProjectExperience> projectExperiences)
    // {
    //     foreach (ProjectExperienceEntity projectExperienceEntity in projectExperiences)
    //     {
    //         await AddToDatabase(projectExperienceEntity);
    //     }
    //
    //     await _db.SaveChangesAsync();
    // }
    //
    // private async Task AddToDatabase(string employeeEmail, Presentation presentation)
    // {
    //     var updatePresentationEntity = await _db.Presentations.SingleOrDefaultAsync(e => e.Id == presentation.Id);
    //
    //     if (updatePresentationEntity != null)
    //     {
    //         updatePresentationEntity.Title = presentation.Title;
    //         updatePresentationEntity.Description = presentation.Description;
    //         updatePresentationEntity.EmployeeId = presentation.EmployeeId;
    //         updatePresentationEntity.Month = presentation.Month;
    //         updatePresentationEntity.Year = presentation.Year;
    //         updatePresentationEntity.Url = presentation.Url;
    //         updatePresentationEntity.Order = presentation.Order;
    //         updatePresentationEntity.LastSynced = DateTime.Now;
    //     }
    //     else
    //     {
    //         await _db.AddAsync(presentation);
    //     }
    // }
    //
    // private async Task AddToDatabase(WorkExperience workExperience)
    // {
    //     var updateWorkExperienceEntity = await _db.WorkExperiences.SingleOrDefaultAsync(e => e.Id == workExperience.Id);
    //     if (updateWorkExperienceEntity != null)
    //     {
    //         updateWorkExperienceEntity.Title = workExperience.Title;
    //         updateWorkExperienceEntity.Description = workExperience.Description;
    //         updateWorkExperienceEntity.MonthFrom = workExperience.MonthFrom;
    //         updateWorkExperienceEntity.MonthTo = workExperience.MonthTo;
    //         updateWorkExperienceEntity.YearFrom = workExperience.YearFrom;
    //         updateWorkExperienceEntity.YearTo = workExperience.YearTo;
    //         updateWorkExperienceEntity.LastSynced = DateTime.Now;
    //     }
    //     else
    //     {
    //         await _db.AddAsync(workExperience);
    //     }
    // }
    //
    // private async Task AddToDatabase(ProjectExperience projectExperience)
    // {
    //     var updateWorkExperienceEntity =
    //         await _db.ProjectExperiences.SingleOrDefaultAsync(e => e.Id == projectExperience.Id);
    //     if (updateWorkExperienceEntity != null)
    //     {
    //         updateWorkExperienceEntity.Title = projectExperience.Title;
    //         updateWorkExperienceEntity.Description = projectExperience.Description;
    //         updateWorkExperienceEntity.MonthFrom = projectExperience.MonthFrom;
    //         updateWorkExperienceEntity.MonthTo = projectExperience.MonthTo;
    //         updateWorkExperienceEntity.YearFrom = projectExperience.YearFrom;
    //         updateWorkExperienceEntity.YearTo = projectExperience.YearTo;
    //         updateWorkExperienceEntity.LastSynced = DateTime.Now;
    //     }
    //     else
    //     {
    //         await _db.AddAsync(projectExperience);
    //     }
    // }
    //
    // public async Task<List<Presentation>> GetPresentationsByEmployeeId(Guid employeeId)
    // {
    //     var thresholdDate = DateTime.Now.AddDays(-30).Date;
    //     return await _db.Presentations.Where(entity => employeeId == entity.EmployeeId)
    //         .Where(e => e.EmployeeId == employeeId && e.LastSynced.Date >= thresholdDate)
    //         .ToListAsync();
    // }
    //
    // public async Task<List<WorkExperience>> GetWorkExperiencesByEmployeeId(Guid employeeId)
    // {
    //     var thresholdDate = DateTime.Now.AddDays(-30).Date;
    //     return await _db.WorkExperiences
    //         .Where(e => e.EmployeeId == employeeId && e.LastSynced.Date >= thresholdDate)
    //         .ToListAsync();
    // }
    //
    // public async Task<List<ProjectExperience>> GetProjectExperiencesByEmployeeId(Guid employeeId)
    // {
    //     var thresholdDate = DateTime.Now.AddDays(-30).Date;
    //     return await _db.ProjectExperiences.Where(entity => employeeId == entity.EmployeeId)
    //         .Where(e => e.EmployeeId == employeeId && e.LastSynced.Date >= thresholdDate)
    //         .ToListAsync();
    // }
}