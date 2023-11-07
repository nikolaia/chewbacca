using System.Net;

using ApplicationCore.Interfaces;
using ApplicationCore.Models;

using Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class EmployeesRepository : IEmployeesRepository
{
    private readonly EmployeeContext _db;
    private readonly ILogger _logger;

    public EmployeesRepository(EmployeeContext db, ILogger<OrchestratorService> logger)
    {
        _db = db;
        _logger = logger;

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

    private async Task<EmployeeEntity?> GetEmployeeWithCv(string email)
    {
        return await _db.Employees
            .Include(employee => employee.ProjectExperiences).ThenInclude(pe => pe.ProjectExperienceRoles).AsSplitQuery()
            .Include(employee => employee.WorkExperiences).AsSplitQuery()
            .Include(employee => employee.Presentations).AsSplitQuery()
            .Include(employee => employee.Certifications).AsSplitQuery()
            .Include(employee => employee.ProjectExperiences).ThenInclude(pe => pe.Competencies).AsSplitQuery()
            .Where(emp => emp.Email == email)
            .SingleOrDefaultAsync();
    }

    private async Task<EmployeeEntity?> GetEmployeeEntityWithCv(string alias, string country)
    {
        return await _db.Employees
            .Include(employee => employee.ProjectExperiences).ThenInclude(pe => pe.ProjectExperienceRoles).AsSplitQuery()
            .Include(employee => employee.WorkExperiences).AsSplitQuery()
            .Include(employee => employee.Presentations).AsSplitQuery()
            .Include(employee => employee.Certifications).AsSplitQuery()
            .Include(employee => employee.ProjectExperiences).ThenInclude(pe => pe.Competencies).AsSplitQuery()
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
            _logger.LogInformation("Starting cv processing of " + cv.Email);
            var entity = await GetEmployeeWithCv(cv.Email);
            if (entity == null)
            {
                continue;
            }

            await AddPresentations(cv.Presentations, entity);
            await AddWorkExperience(cv.WorkExperiences, entity);
            await AddProjectExperience(cv.ProjectExperiences, entity);
            await AddCertifications(cv.Certifiactions, entity);
            await _db.SaveChangesAsync();
        }
    }

    private async Task AddCertifications(List<Certification> certifications, EmployeeEntity entity)
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
                    IssuedDate = certification.IssuedDate,
                    ExpiryDate = certification.ExpiryDate,
                    Issuer = certification.Issuer,
                    LastSynced = DateTime.Now,
                    Employee = entity
                };
                await _db.AddAsync(certificationEntity);
            }
            else
            {
                certificationEntity.Title = certification.Title;
                certificationEntity.Description = certification.Description;
                certificationEntity.ExpiryDate = certification.ExpiryDate;
                certificationEntity.IssuedDate = certification.IssuedDate;
                certificationEntity.LastSynced = DateTime.Now;
            }
            await _db.SaveChangesAsync();
        }
    }

    private async Task AddPresentations(List<Presentation> presentations, EmployeeEntity entity)
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
                    Date = presentation.Date,
                    Title = presentation.Title,
    
                };
                await _db.AddAsync(presentationEntity);

            }
            else
            {

                presentationEntity.Description = presentation.Description;
                presentationEntity.Date = presentation.Date;
                presentationEntity.Title = presentation.Title;
                presentationEntity.LastSynced = DateTime.Now;
            }
        }
    }

    private async Task AddWorkExperience(List<WorkExperience> workExperiences, EmployeeEntity entity)
    {
        foreach (WorkExperience workExperience in workExperiences)
        {
            var workExperienceEntity = entity.WorkExperiences.SingleOrDefault(p => p.Id == workExperience.Id);
            if (workExperienceEntity == null)
            {
                workExperienceEntity = new WorkExperienceEntity()
                {
                    Id = workExperience.Id,
                    EmployeeId = entity.Id,
                    Description = workExperience.Description,
                    FromDate = workExperience.FromDate,
                    ToDate = workExperience.ToDate,
                    Title = workExperience.Title,
                    Company = workExperience.Company,
                    LastSynced = DateTime.Now
                };
                await _db.AddAsync(workExperienceEntity);
            }
            else
            {
                workExperienceEntity.Description = workExperience.Description;
                workExperienceEntity.Title = workExperience.Title;
                workExperienceEntity.FromDate = workExperience.FromDate;
                workExperienceEntity.ToDate = workExperience.ToDate;
                workExperienceEntity.Company = workExperience.Company;
                workExperienceEntity.LastSynced = DateTime.Now;
            }
        }

    }

    private async Task AddProjectExperience(List<ProjectExperience> projectExperiences,
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
                    FromDate = projectExperience.FromDate,
                    ToDate = projectExperience.ToDate,
                    Customer = projectExperience.Customer,
                    LastSynced = DateTime.Now,
                };
                await _db.AddAsync(projectExperienceEntity);
            }
            else
            {
                projectExperienceEntity.Description = projectExperience.Description;
                projectExperienceEntity.Title = projectExperience.Title;
                projectExperienceEntity.FromDate = projectExperience.FromDate;
                projectExperienceEntity.ToDate = projectExperience.ToDate;
                projectExperienceEntity.Customer = projectExperience.Customer;
                projectExperienceEntity.LastSynced = DateTime.Now;
            }
            await AddProjectExperienceRole(projectExperience.Roles, projectExperienceEntity);
            await AddCompetencies(projectExperience.Competencies, projectExperienceEntity);
        }
    }

    private async Task AddCompetencies(HashSet<string> competencies, ProjectExperienceEntity projectExperienceEntity)
    {

        foreach (string competency in competencies)
        {
            var competencyEntity = projectExperienceEntity.Competencies.SingleOrDefault(c => c.Name == competency);
            if (competencyEntity == null)
            {
                competencyEntity = new CompetencyEntity { Name = competency, LastSynced = DateTime.Now, ProjectExperience = projectExperienceEntity };
                await _db.AddAsync(competencyEntity);
            }
            else
            {
                competencyEntity.LastSynced = DateTime.Now;
            }
        }
    }

    private async Task AddProjectExperienceRole(List<ProjectExperienceRole> projectExperienceRoles,
        ProjectExperienceEntity projectExperienceEntity)
    {
        foreach (ProjectExperienceRole projectExperienceRole in projectExperienceRoles)
        {
            var projectExperienceRoleEntity = projectExperienceEntity.ProjectExperienceRoles.SingleOrDefault(per => per.Id == projectExperienceRole.Id);
            if (projectExperienceRoleEntity == null)
            {
                projectExperienceRoleEntity = new ProjectExperienceRoleEntity
                {
                    Description = projectExperienceRole.Description,
                    Title = projectExperienceRole.Title,
                    Id = projectExperienceRole.Id,
                    LastSynced = DateTime.Now,
                    ProjectExperience = projectExperienceEntity
                };
                await _db.AddAsync(projectExperienceRoleEntity);
            }
            else
            {
                projectExperienceRoleEntity.Description = projectExperienceRole.Description;
                projectExperienceRoleEntity.Title = projectExperienceRole.Title;
                projectExperienceRoleEntity.LastSynced = DateTime.Now;
            }
        }

    }

    private void AddCompetenciesToProjects(ProjectExperience pe)
    {
        pe.Competencies.UnionWith(_db.Competencies.Where(c => c.ProjectExperienceId == pe.Id).Select(c => c.Name).Distinct().ToHashSet());
    }

    public async Task<Cv> GetEmployeeWithCv(string alias, string country)
    {
        var entity = await GetEmployeeEntityWithCv(alias, country) ?? throw new HttpRequestException("not found", null, HttpStatusCode.NotFound);
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

    public async Task<List<ProjectExperience>> GetProjectExperiencesByEmailAndCompetencies(string email,
        List<string> competencies)
    {
        var employeeId = await _db.Employees
            .Where(emp => emp.Email == email)
            .Select(e => e.Id)
            .SingleOrDefaultAsync();

        return await _db.ProjectExperiences
            .Include(pe => pe.ProjectExperienceRoles)
            .Include(pe => pe.Competencies)
            .Where(pe =>
                pe.EmployeeId == employeeId &&
                _db.Competencies.Any(c => competencies.Contains(c.Name) && c.ProjectExperienceId == pe.Id))
            .Select(pe => pe.ToProjectExperience())
            .ToListAsync();
    }

    public async Task<List<string>> GetAllCompetencies(string? email)
    {
        return email == null
            ? await _db.Competencies.Select(entity => entity.Name).Distinct().ToListAsync()
            : await GetAllCompetenciesForEmployee(email);
    }

    private async Task<List<string>> GetAllCompetenciesForEmployee(string email)
    {
        return await _db.Employees.Where(emp => emp.Email == email)
        .Join(_db.ProjectExperiences, emp => emp.Id, project => project.EmployeeId, (emp, project) => project)
        .Join(_db.Competencies, project => project.Id, comp => comp.ProjectExperienceId, (project, comp) => comp.Name).Distinct().ToListAsync();
    }
}