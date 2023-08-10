using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Services;

using Infrastructure.ApiClients.DTOs;
using Infrastructure.Entities;
using Infrastructure.Repositories;

using Microsoft.Extensions.Logging;

using PhoneNumbers;

using Shared;

namespace Infrastructure;

public class OrchestratorService
{
    private readonly IEmployeesRepository _employeesRepository;
    private readonly CvPartnerRepository _cvPartnerRepository;
    private readonly IBemanningRepository _bemanningRepository;
    private readonly BlobStorageService _blobStorageService;
    private readonly ILogger<OrchestratorService> _logger;
    private readonly FilteredUids _filteredUids;

    public OrchestratorService(CvPartnerRepository cvPartnerRepository,
        IBemanningRepository bemanningRepository,
        IEmployeesRepository employeesRepository,
        BlobStorageService blobStorageService,
        FilteredUids filteredUids,
        ILogger<OrchestratorService> logger)
    {
        _employeesRepository = employeesRepository;
        _cvPartnerRepository = cvPartnerRepository;
        _bemanningRepository = bemanningRepository;
        _blobStorageService = blobStorageService;
        _logger = logger;
        _filteredUids = filteredUids;
    }

    public async Task FetchMapAndSaveEmployeeData()
    {
        _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveEmployeeData: Started");
        var bemanningEntries = await _bemanningRepository.GetBemanningDataForEmployees();
        var cvEntries = await _cvPartnerRepository.GetAllEmployees();

        var phoneNumberUtil = PhoneNumberUtil.GetInstance();

        foreach (var bemanning in bemanningEntries.Where(IsActiveEmployee))
        {
            var cv = cvEntries.Find(cv => cv.email.ToLower().Trim() == bemanning.Email.ToLower().Trim());

            if (cv != null)
            {
                var countryCode = cv.email.ToLower().EndsWith(".se") ? "SE" : "NO";

                var phoneNumber = phoneNumberUtil.IsPossibleNumber(cv.telephone, countryCode)
                    ? phoneNumberUtil.Format(phoneNumberUtil.Parse(cv.telephone, countryCode),
                        PhoneNumberFormat.E164)
                    : null;

                var isFilteredPhone = _filteredUids.Uids.Contains(cv.user_id);

                await _employeesRepository.AddToDatabase(new Employee
                {
                    Name = cv.name,
                    Email = cv.email,
                    Telephone = isFilteredPhone ? null : phoneNumber,
                    ImageUrl =
                        cv.image.url != null
                            ? await _blobStorageService.SaveToBlob(cv.user_id, cv.image.url)
                            : null,
                    OfficeName = cv.office_name,
                    StartDate = bemanning.StartDate,
                    EndDate = bemanning.EndDate,
                    CountryCode = countryCode
                });
            }
            else
            {
                // If the employee does not exist in CV Partner, only in Bemanning, we should ensure the employee is not in the database.
                _logger.LogInformation(
                    "Deleting employee with email {BemanningEmail} from database, since it does not exist in CV Partner",
                    bemanning.Email);
                var blobUrlToBeDeleted = await _employeesRepository.EnsureEmployeeIsDeleted(bemanning.Email);
                if (blobUrlToBeDeleted == null)
                {
                    continue;
                }

                _logger.LogInformation("Deleting blob with url {BlobUrlToBeDeleted}", blobUrlToBeDeleted);
                await _blobStorageService.DeleteBlob(blobUrlToBeDeleted);
            }
        }

        var blobUrlsToBeDeleted = await _employeesRepository.EnsureEmployeesWithEndDateBeforeTodayAreDeleted();
        foreach (var blobUrlToBeDeleted in blobUrlsToBeDeleted)
        {
            _logger.LogInformation("Deleting blob with url {BlobUrlToBeDeleted}", blobUrlToBeDeleted);
            if (blobUrlToBeDeleted != null)
            {
                await _blobStorageService.DeleteBlob(blobUrlToBeDeleted);
            }
        }

        _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveEmployeeData: Finished");
    }

    private static bool IsActiveEmployee(BemanningEmployee bemanning)
    {
        return DateTime.Now >= bemanning.StartDate && (bemanning.EndDate == null || DateTime.Now <= bemanning.EndDate);
    }


    // public async Task FetchMapAndSaveCvData()
    // {
    //     _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveCVData: Started");
    //     var employeeEntries = await _employeesService.GetActiveEmployees();
    //     var userEntries = await _cvPartnerRepository.GetCvPartnerEmployees();
    //     List<PresentationEntity> presentations = new();
    //     List<WorkExperienceEntity> workExperiences = new();
    //     List<ProjectExperienceEntity> projectExperiences = new();
    //     
    //     
    //     foreach (var employee in employeeEntries)
    //     {
    //         var user = userEntries.Find(cv => cv.email.ToLower().Trim() == employee.Email.ToLower().Trim());
    //
    //         if (user == null)
    //         {
    //             continue;
    //         }
    //
    //         {
    //             var cv = await _cvPartnerRepository.GetCvForEmployee(user.user_id, user.default_cv_id);
    //             presentations.AddRange(CreatePresentationsFromCv(cv, employee));
    //             workExperiences.AddRange(CreateWorkExperienceFromCv(cv, employee));
    //             projectExperiences.AddRange(CreateProjectExperienceFromCv(cv, employee));
    //         }
    //     }
    //
    //     await _employeesRepository.AddToDatabase(presentations);
    //     await _employeesRepository.AddToDatabase(workExperiences);
    //     await _employeesRepository.AddToDatabase(projectExperiences);
    //     _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveCvData: Finished");
    // }

    private static IEnumerable<PresentationEntity> CreatePresentationsFromCv(CVPartnerCvDTO cv, EmployeeEntity employee)
    {
        if (cv.presentations == null)
        {
            return new List<PresentationEntity>();
        }

        return cv.presentations.Select(dto => new PresentationEntity
        {
            Id = dto._id,
            EmployeeId = employee.Id,
            Description = dto.long_description.no ?? "",
            Year = dto.year,
            Month = dto.month,
            Title = dto.description.no ?? "",
            LastSynced = DateTime.Now
        });
    }

    private static IEnumerable<WorkExperienceEntity> CreateWorkExperienceFromCv(CVPartnerCvDTO cv, EmployeeEntity employee)
    {
        if (cv.work_experiences == null)
        {
            return new List<WorkExperienceEntity>();
        }

        return cv.work_experiences.Select(dto => new WorkExperienceEntity()
        {
            Id = dto._id,
            EmployeeId = employee.Id,
            Description = dto.long_description.no ?? "",
            MonthFrom = dto.month_from,
            YearFrom = dto.year_from,
            MonthTo = dto.month_to,
            YearTo = dto.year_to,
            Title = dto.description.no ?? "",
            LastSynced = DateTime.Now
        });
    }

    private static IEnumerable<ProjectExperienceEntity> CreateProjectExperienceFromCv(CVPartnerCvDTO cv,
        EmployeeEntity employee)
    {
        if (cv.project_experiences == null)
        {
            return new List<ProjectExperienceEntity>();
        }

        return cv.project_experiences.Select(dto => new ProjectExperienceEntity()
        {
            Id = dto._id,
            EmployeeId = employee.Id,
            Description = dto.long_description.no ?? "",
            MonthFrom = dto.month_from,
            YearFrom = dto.year_from,
            MonthTo = dto.month_to,
            YearTo = dto.year_to, 
            Title = dto.description.no ?? "",
            LastSynced = DateTime.Now
        });
    }
    
}