using System.Net;

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

    private async Task<EmployeeEntity?> GetEmployeeEntity(string email)
    {
        return await _db.Employees
            .Include(employee => employee.ProjectExperiences)
            .ThenInclude(entity => entity.ProjectExperienceRoles)
            .Include(employee => employee.WorkExperiences)
            .Include(employee => employee.Presentations)
            .Where(emp => emp.Email == email)
            .SingleOrDefaultAsync();
    }

    private async Task<EmployeeEntity?> GetEmployeeEntityWithCv(string alias, string country)
    {
        return await _db.Employees
            .Include(employee => employee.ProjectExperiences)
            .ThenInclude(entity => entity.ProjectExperienceRoles)
            .Include(employee => employee.WorkExperiences)
            .Include(employee => employee.Presentations)
            .Include(employee => employee.Certifications)
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


    public async Task AddOrUpdateEmployeeInformation(Employee employee)
    {
        EmployeeInformation employeeInformation = employee.EmployeeInformation;
        EmployeeEntity? updateEmployee =
            await _db.Employees.SingleOrDefaultAsync(e => e.Email == employeeInformation.Email);

        if (updateEmployee != null)
        {
            updateEmployee.Email = employeeInformation.Email;
            updateEmployee.Name = employeeInformation.Name;
            updateEmployee.ImageUrl = employeeInformation.ImageUrl;
            updateEmployee.Telephone = employeeInformation.Telephone;
            updateEmployee.OfficeName = employeeInformation.OfficeName;
            updateEmployee.StartDate = employeeInformation.StartDate;
            updateEmployee.EndDate = employeeInformation.EndDate;
            // Don't set Address, AccountNumber, ZipCode and City since these aren't fetched from external sources,
            // and hence the information given from variantdash will be overwritten
        }
        else
        {
            await _db.AddAsync(new EmployeeEntity
            {
                Email = employeeInformation.Email,
                Name = employeeInformation.Name,
                ImageUrl = employeeInformation.ImageUrl,
                Telephone = employeeInformation.Telephone,
                OfficeName = employeeInformation.OfficeName,
                StartDate = employeeInformation.StartDate,
                EndDate = employeeInformation.EndDate,
                CountryCode = employeeInformation.CountryCode
            });
        }

        await _db.SaveChangesAsync();
    }

    public async Task<bool> UpdateEmployeeInformation(string alias, string country,
        UpdateEmployeeInformation employeeInformation)
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

    public async Task AddOrUpdateCvInformation(List<Cv> cvs)
    {
        foreach (Cv cv in cvs)
        {
            var entity = await GetEmployeeEntity(cv.Email);
            if (entity == null)
            {
                continue;
            }

            await AddPresentationsUncommitted(cv.Presentations, entity);
            await AddWorkExperienceUncommitted(cv.WorkExperiences, entity);
            await AddProjectExperienceUncommitted(cv.ProjectExperiences, entity);
            await AddCertificationUncommitted(cv.Certifiactions, entity);
        }

        await _db.SaveChangesAsync();
    }

    private async Task AddCertificationUncommitted(List<Certification> certifications, EmployeeEntity entity)
    {
        foreach (Certification certification in certifications)
        {
            var certificationEntity = entity.Certifications.SingleOrDefault(c => c.Id == certification.Id);
            if (certificationEntity == null)
            {
                certificationEntity = new CertificationEntity
                {
                    Id = certification.Id,
                    Description = certification.Description,
                    Title = certification.Title,
                    IssuedMonth = certification.IssuedMonth,
                    IssuedYear = certification.IssuedYear,
                    ExpiryDate = certification.ExpiryDate,
                    LastSynced = DateTime.Now,
                    Employee = entity
                };
                await _db.AddAsync(certificationEntity);
                continue;
            }

            certificationEntity.Title = certification.Title;
            certificationEntity.Description = certification.Description;
            certificationEntity.ExpiryDate = certification.ExpiryDate;
            certificationEntity.IssuedMonth = certification.IssuedMonth;
            certificationEntity.IssuedYear = certification.IssuedYear;
            certificationEntity.LastSynced = DateTime.Now;
        }
    }

    private async Task AddPresentationsUncommitted(List<Presentation> presentations, EmployeeEntity entity)
    {
        foreach (Presentation presentation in presentations)
        {
            var presentationEntity = entity.Presentations.SingleOrDefault(p => p.Id == presentation.Id);
            if (presentationEntity == null)
            {
                presentationEntity = new PresentationEntity
                {
                    Id = presentation.Id,
                    Employee = entity,
                    Description = presentation.Description ?? "",
                    LastSynced = DateTime.Now,
                    Month = presentation.Month,
                    Title = presentation.Title,
                    Year = presentation.Year
                };
                await _db.AddAsync(presentationEntity);
                continue;
            }

            presentationEntity.Description = presentation.Description;
            presentationEntity.Month = presentation.Month;
            presentationEntity.Year = presentation.Year;
            presentationEntity.Title = presentation.Title;
            presentationEntity.LastSynced = DateTime.Now;
        }
    }

    private async Task AddWorkExperienceUncommitted(List<WorkExperience> workExperiences, EmployeeEntity entity)
    {
        foreach (WorkExperience workExperience in workExperiences)
        {
            var workExperienceEntity = entity.WorkExperiences.SingleOrDefault(p => p.Id == workExperience.Id);
            if (workExperienceEntity == null)
            {
                workExperienceEntity = new WorkExperienceEntity()
                {
                    Id = workExperience.Id,
                    Employee = entity,
                    Description = workExperience.Description,
                    MonthFrom = workExperience.MonthFrom,
                    MonthTo = workExperience.MonthTo,
                    YearFrom = workExperience.YearFrom,
                    YearTo = workExperience.YearTo,
                    Title = workExperience.Title,
                    LastSynced = DateTime.Now
                };
                await _db.AddAsync(workExperienceEntity);
                continue;
            }

            workExperienceEntity.Description = workExperience.Description;
            workExperienceEntity.Title = workExperience.Title;
            workExperienceEntity.MonthFrom = workExperience.MonthFrom;
            workExperienceEntity.MonthTo = workExperience.MonthTo;
            workExperienceEntity.YearFrom = workExperience.YearFrom;
            workExperienceEntity.YearTo = workExperience.YearTo;
            workExperienceEntity.LastSynced = DateTime.Now;
        }
    }

    private async Task AddProjectExperienceUncommitted(List<ProjectExperience> projectExperiences,
        EmployeeEntity entity)
    {
        foreach (ProjectExperience projectExperience in projectExperiences)
        {

            var projectExperienceEntity =
                entity.ProjectExperiences.SingleOrDefault(p => p.Id == projectExperience.Id);
            if (projectExperienceEntity == null)
            {
                projectExperienceEntity = new ProjectExperienceEntity()
                {
                    Id = projectExperience.Id,
                    Employee = entity,
                    Description = projectExperience.Description,
                    Title = projectExperience.Title,
                    MonthFrom = projectExperience.MonthFrom,
                    MonthTo = projectExperience.MonthTo,
                    YearFrom = projectExperience.YearFrom,
                    YearTo = projectExperience.YearTo,
                    LastSynced = DateTime.Now,
                };
                await _db.AddAsync(projectExperienceEntity);
            }
            else
            {
                projectExperienceEntity.Description = projectExperience.Description;
                projectExperienceEntity.Title = projectExperience.Title;
                projectExperienceEntity.MonthFrom = projectExperience.MonthFrom;
                projectExperienceEntity.MonthTo = projectExperience.MonthTo;
                projectExperienceEntity.YearFrom = projectExperience.YearFrom;
                projectExperienceEntity.YearTo = projectExperience.YearTo;
                projectExperienceEntity.LastSynced = DateTime.Now;
            }
            
            await AddProjectExperienceRoleUncommitted(projectExperience.Roles, projectExperienceEntity);
        }
    }

    private async Task AddProjectExperienceRoleUncommitted(List<ProjectExperienceRole> projectExperienceRoles,
        ProjectExperienceEntity projectExperienceEntity)
    {
        foreach (ProjectExperienceRole projectExperienceRole in projectExperienceRoles)
        {
            var projectExperienceRoleEntity =
                projectExperienceEntity.ProjectExperienceRoles.SingleOrDefault(e => e.Id == projectExperienceRole.Id);
            if (projectExperienceRoleEntity == null)
            {
                projectExperienceRoleEntity = new ProjectExperienceRoleEntity()
                {
                    Description = projectExperienceRole.Description,
                    Title = projectExperienceRole.Title,
                    Id = projectExperienceRole.Id,
                    LastSynced = DateTime.Now,
                    ProjectExperience = projectExperienceEntity
                };
                await _db.AddAsync(projectExperienceRoleEntity);
                continue;
            }

            projectExperienceRoleEntity.Description = projectExperienceRole.Description;
            projectExperienceRoleEntity.Title = projectExperienceRole.Title;
            projectExperienceRoleEntity.LastSynced = DateTime.Now;

        }
    }
    

    public async Task<Cv> GetEmployeeWithCv(string alias, string country)
    {
        var entity = await GetEmployeeEntityWithCv(alias, country);
        if (entity == null)
        {
            throw new HttpRequestException("not found", null, HttpStatusCode.NotFound);
        }

        return entity.ToCv();
    }

    private async Task<EmergencyContact?> SetEmergencyContactAsync(Employee employee)
    {
        var emergencyContact = await _db.EmergencyContacts
            .Where(emp => emp.Employee.Email.Equals(employee.EmployeeInformation.Email))
            .SingleOrDefaultAsync();
        return emergencyContact?.ToEmergencyContact();
    }

    public async Task AddOrUpdateEmployeeInformation(string employeeEmail, EmergencyContact emergencyContact)
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
}