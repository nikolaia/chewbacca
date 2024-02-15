using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Services;

using Infrastructure.ApiClients.DTOs;
using Infrastructure.Repositories;

using Microsoft.Extensions.Logging;

using PhoneNumbers;

using Shared;

namespace Infrastructure;

public class OrchestratorService
{
    private readonly IEmployeesRepository _employeesRepository;
    private readonly CvPartnerRepository _cvPartnerRepository;
    private readonly IVibesRepository _vibesRepository;
    private readonly BlobStorageService _blobStorageService;
    private readonly ILogger<OrchestratorService> _logger;
    private readonly FilteredUids _filteredUids;

    public OrchestratorService(CvPartnerRepository cvPartnerRepository,
        IVibesRepository vibesRepository,
        IEmployeesRepository employeesRepository,
        BlobStorageService blobStorageService,
        FilteredUids filteredUids,
        ILogger<OrchestratorService> logger)
    {
        _employeesRepository = employeesRepository;
        _cvPartnerRepository = cvPartnerRepository;
        _vibesRepository = vibesRepository;
        _blobStorageService = blobStorageService;
        _logger = logger;
        _filteredUids = filteredUids;
    }

    public async Task FetchMapAndSaveEmployeeData()
    {
        _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveEmployeeData: Started");
        var bemanningEntries = await _vibesRepository.GetEmployment();
        var cvEntries = await _cvPartnerRepository.GetAllEmployees();

        var phoneNumberUtil = PhoneNumberUtil.GetInstance();

        foreach (var bemanning in bemanningEntries.Where(IsActiveEmployee))
        {
            var cv = cvEntries.Find(cv => cv.email.ToLower().Trim() == bemanning.email.ToLower().Trim());

            if (cv != null)
            {
                var countryCode = cv.email.ToLower().EndsWith(".se") ? "SE" : "NO";

                var phoneNumber = phoneNumberUtil.IsPossibleNumber(cv.telephone, countryCode)
                    ? phoneNumberUtil.Format(phoneNumberUtil.Parse(cv.telephone, countryCode),
                        PhoneNumberFormat.E164)
                    : null;

                var isFilteredPhone = _filteredUids.Uids.Contains(cv.user_id);

                await _employeesRepository.AddOrUpdateEmployeeInformation(new Employee
                {
                    EmployeeInformation = new EmployeeInformation()
                    {
                        Name = cv.name,
                        Email = cv.email,
                        Telephone = isFilteredPhone ? null : phoneNumber,
                        ImageUrl =
                            cv.image.url != null
                                ? await _blobStorageService.SaveToBlob(cv.user_id, cv.image.url)
                                : null,
                        OfficeName = cv.office_name,
                        StartDate = bemanning.startDate,
                        EndDate = bemanning.endDate,
                        CountryCode = countryCode
                    }
                });
            }
            else
            {
                // If the employee does not exist in CV Partner, only in Bemanning, we should ensure the employee is not in the database.
                _logger.LogInformation(
                    "Deleting employee with email {BemanningEmail} from database, since it does not exist in CV Partner",
                    bemanning.email);
                var blobUrlToBeDeleted = await _employeesRepository.EnsureEmployeeIsDeleted(bemanning.email);
                if (blobUrlToBeDeleted == null)
                {
                    continue;
                }

                _logger.LogInformation("Deleting blob with url {BlobUrlToBeDeleted}", blobUrlToBeDeleted);
                await _blobStorageService.DeleteBlob(blobUrlToBeDeleted);
            }
        }

        // Remove employees with end date that has been passed.
        var blobUrlsToBeDeleted = (await _employeesRepository.EnsureEmployeesWithEndDateBeforeTodayAreDeleted()).ToList();

        foreach (var bemanning in bemanningEntries.Where(IsFutureEmployee))
        {
            // Remove potential employees that shouldn't have been added.
            // This should normally not happen, but might happen in cases where
            // StartDate in bemanning wasn't set properly when orchestrating. Covering an edge case
            _logger.LogInformation(
                "Deleting employee with email {BemanningEmail} from database, since they haven't started yet",
                bemanning.email);
            blobUrlsToBeDeleted.Add(await _employeesRepository.EnsureEmployeeIsDeleted(bemanning.email));
        }

        // Remove all potential images from both past employees and future employees
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

    private static bool IsActiveEmployee(VibesEmploymentDTO employmentDto)
    {
        return DateTime.Now >= employmentDto.startDate && (employmentDto.endDate == null || DateTime.Now <= employmentDto.endDate);
    }

    private static bool IsFutureEmployee(VibesEmploymentDTO employmentDto)
    {
        return employmentDto.startDate > DateTime.Now;
    }

    public async Task FetchMapAndSaveCvData()
    {
        _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveCVData: Started");
        var employeeEntries = await _employeesRepository.GetAllEmployees();
        var userEntries = await _cvPartnerRepository.GetAllEmployees();
        List<Cv> cvs = new();

        foreach (var user in employeeEntries.Select(employee => userEntries.Find(cv =>
                     cv.email.ToLower().Trim() == employee.EmployeeInformation.Email.ToLower().Trim())).Where(user => user != null))
        {
            var cvPartnerCv = await _cvPartnerRepository.GetEmployeeCv(user.user_id, user.default_cv_id);
            cvs.Add(CvDtoConverter.ToCv(cvPartnerCv));
        }

        await _employeesRepository.AddOrUpdateCvInformation(cvs);
        _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveCvData: Finished");
    }

}