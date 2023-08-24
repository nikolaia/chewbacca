using Employees.Models;

using Microsoft.EntityFrameworkCore;

namespace Employees.Repositories;

public class EmployeesRepository
{
    private readonly EmployeeContext _db;

    public EmployeesRepository(EmployeeContext db)
    {
        _db = db;
    }

    public async Task<List<EmployeeEntity>> GetAllEmployees()
    {
        return await _db.Employees.ToListAsync();
    }

    public async Task<List<EmployeeEntity>> GetEmployeesByCountry(string country)
    {
        return await _db.Employees.Where(emp => emp.CountryCode == country).ToListAsync();
    }

    public async Task<EmployeeEntity?> GetEmployeeAsync(string alias, string country)
    {
        return await _db.Employees
            .Include(employee => employee.AllergiesAndDietaryPreferences)
            .Include(employee => employee.EmergencyContact)
            .Where(emp => emp.Email.StartsWith($"{alias}@"))
            .Where(emp => emp.CountryCode == country)
            .SingleOrDefaultAsync();
    }

    public async Task AddToDatabase(EmployeeEntity employee)
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
            updateEmployee.CountryCode = employee.CountryCode;
            // Don't set Address, AccountNumber, ZipCode and City since these aren't fetched from external sources,
            // and hence the information given from variantdash will be overwritten
        }
        else
        {
            await _db.AddAsync(employee);
        }

        await _db.SaveChangesAsync();
    }

    public async Task<bool> UpdateEmployeeInformation(string alias, string country,
        EmployeeInformation employeeInformation)
    {
        var employee = await GetEmployeeAsync(alias, country);

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

    public async Task<EmergencyContactEntity?> GetEmergencyContactAsync(EmployeeEntity employee)
    {
        return await _db.EmergencyContacts
            .Where(emp => emp.Employee.Equals(employee))
            .SingleOrDefaultAsync();
    }

    public async Task AddToDatabase(EmergencyContactEntity emergencyContact)
    {
        var updateEmergencyContact =
            await _db.EmergencyContacts.SingleOrDefaultAsync(e => e.Employee == emergencyContact.Employee);

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

    public async Task AddToDatabase(List<PresentationEntity> presentations)
    {
        foreach (PresentationEntity presentationEntity in presentations)
        {
            await AddToDatabase(presentationEntity);
        }

        await _db.SaveChangesAsync();
    }

    public async Task AddToDatabase(List<WorkExperienceEntity> presentations)
    {
        foreach (WorkExperienceEntity presentationEntity in presentations)
        {
            await AddToDatabase(presentationEntity);
        }

        await _db.SaveChangesAsync();
    }

    public async Task AddToDatabase(List<ProjectExperienceEntity> projectExperiences)
    {
        foreach (ProjectExperienceEntity projectExperienceEntity in projectExperiences)
        {
            await AddToDatabase(projectExperienceEntity);
        }

        await _db.SaveChangesAsync();
    }

    public async Task AddToDatabase(List<ProjectExperienceRoleEntity> projectExperienceRoleEntities)
    {
        foreach (ProjectExperienceRoleEntity projectExperienceRoleEntity in projectExperienceRoleEntities)
        {
            await AddToDatabase(projectExperienceRoleEntity);
        }

        await _db.SaveChangesAsync();
    }
    private async Task AddToDatabase(PresentationEntity presentation)
    {
        var updatePresentationEntity = await _db.Presentations.SingleOrDefaultAsync(e => e.Id == presentation.Id);

        if (updatePresentationEntity != null)
        {
            updatePresentationEntity.Title = presentation.Title;
            updatePresentationEntity.Description = presentation.Description;
            updatePresentationEntity.EmployeeId = presentation.EmployeeId;
            updatePresentationEntity.Month = presentation.Month;
            updatePresentationEntity.Year = presentation.Year;
            updatePresentationEntity.Url = presentation.Url;
            updatePresentationEntity.Order = presentation.Order;
            updatePresentationEntity.LastSynced = DateTime.Now;
        }
        else
        {
            await _db.AddAsync(presentation);
        }
    }

    private async Task AddToDatabase(WorkExperienceEntity workExperience)
    {
        var updateWorkExperienceEntity = await _db.WorkExperiences.SingleOrDefaultAsync(e => e.Id == workExperience.Id);
        if (updateWorkExperienceEntity != null)
        {
            updateWorkExperienceEntity.Title = workExperience.Title;
            updateWorkExperienceEntity.Description = workExperience.Description;
            updateWorkExperienceEntity.MonthFrom = workExperience.MonthFrom;
            updateWorkExperienceEntity.MonthTo = workExperience.MonthTo;
            updateWorkExperienceEntity.YearFrom = workExperience.YearFrom;
            updateWorkExperienceEntity.YearTo = workExperience.YearTo;
            updateWorkExperienceEntity.LastSynced = DateTime.Now;
        }
        else
        {
            await _db.AddAsync(workExperience);
        }
    }

    private async Task AddToDatabase(ProjectExperienceEntity projectExperience)
    {
        var updateWorkExperienceEntity =
            await _db.ProjectExperiences.SingleOrDefaultAsync(e => e.Id == projectExperience.Id);
        if (updateWorkExperienceEntity != null)
        {
            updateWorkExperienceEntity.Title = projectExperience.Title;
            updateWorkExperienceEntity.Description = projectExperience.Description;
            updateWorkExperienceEntity.MonthFrom = projectExperience.MonthFrom;
            updateWorkExperienceEntity.MonthTo = projectExperience.MonthTo;
            updateWorkExperienceEntity.YearFrom = projectExperience.YearFrom;
            updateWorkExperienceEntity.YearTo = projectExperience.YearTo;
            updateWorkExperienceEntity.LastSynced = DateTime.Now;
        }
        else
        {
            await _db.AddAsync(projectExperience);
        }
    }
    
    private async Task AddToDatabase(ProjectExperienceRoleEntity role)
    {
        var updateRoleEntity =
            await _db.ProjectExperienceRoles.SingleOrDefaultAsync(e => e.Id == role.Id);
        if (updateRoleEntity != null)
        {
            updateRoleEntity.title = role.title;
            updateRoleEntity.description = role.description;
            updateRoleEntity.LastSynced = DateTime.Now;
        }
        else
        {
            await _db.AddAsync(role);
        }
    }
    
    

    public async Task<List<PresentationEntity>> GetPresentationsByEmployeeId(Guid employeeId)
    {
        var thresholdDate = DateTime.Now.AddDays(-30).Date;
        return await _db.Presentations.Where(entity => employeeId == entity.EmployeeId)
            .Where(e => e.EmployeeId == employeeId )
            .Where(e => e.LastSynced.Date >= thresholdDate)
            .ToListAsync();
    }

    public async Task<List<WorkExperienceEntity>> GetWorkExperiencesByEmployeeId(Guid employeeId)
    {
        var thresholdDate = DateTime.Now.AddDays(-30).Date;
        return await _db.WorkExperiences
            .Where(e => e.EmployeeId == employeeId)
            .Where(e => e.LastSynced.Date >= thresholdDate)
            .ToListAsync();
    }
    
    public async Task<List<ProjectExperienceEntity>> GetProjectExperiencesByEmployeeId(Guid employeeId)
    {
        var thresholdDate = DateTime.Now.AddDays(-30).Date;
        return await _db.ProjectExperiences.Where(entity => employeeId == entity.EmployeeId)
            .Where(e =>  e.LastSynced.Date >= thresholdDate)
            .ToListAsync();
    }


    public async Task<List<ProjectExperienceRoleEntity>> GetProjectRoleByProjectId(string? projectId)
    {
        var thresholdDate = DateTime.Now.AddDays(-30).Date;
        return await _db.ProjectExperienceRoles.Where(entity => projectId == entity.projectId)
            .Where(e => e.LastSynced.Date >= thresholdDate)
            .ToListAsync();
    }
}