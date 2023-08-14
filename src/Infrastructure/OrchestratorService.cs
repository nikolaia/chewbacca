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
                        StartDate = bemanning.StartDate,
                        EndDate = bemanning.EndDate,
                        CountryCode = countryCode
                    }
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

        // Remove employees with end date that has been passed.
        var blobUrlsToBeDeleted = (await _employeesRepository.EnsureEmployeesWithEndDateBeforeTodayAreDeleted()).ToList();

        foreach (var bemanning in bemanningEntries.Where(IsFutureEmployee))
        {
            // Remove potential employees that shouldn't have been added.
            // This should normally not happen, but might happen in cases where
            // StartDate in bemanning wasn't set properly when orchestrating. Covering an edge case
            _logger.LogInformation(
                "Deleting employee with email {BemanningEmail} from database, since they haven't started yet",
                bemanning.Email);
            blobUrlsToBeDeleted.Add(await _employeesRepository.EnsureEmployeeIsDeleted(bemanning.Email));
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

    private static bool IsActiveEmployee(BemanningEmployee bemanning)
    {
        return DateTime.Now >= bemanning.StartDate && (bemanning.EndDate == null || DateTime.Now <= bemanning.EndDate);
    }

    private static bool IsFutureEmployee(BemanningEmployee bemanning)
    {
        return bemanning.StartDate > DateTime.Now;
    }

    public async Task FetchMapAndSaveCvData()
    {
        _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveCVData: Started");
        var employeeEntries = await _employeesRepository.GetAllEmployees();
        var userEntries = await _cvPartnerRepository.GetAllEmployees();

        foreach (var employee in employeeEntries)
        {
            var user = userEntries.Find(cv => cv.email.ToLower().Trim() == employee.EmployeeInformation.Email.ToLower().Trim());

            if (user == null)
            {
                continue;
            }

            {
                var cv = await _cvPartnerRepository.GetEmployeeCv(user.user_id, user.default_cv_id);
                employee.Cv = CvDtoConverter.ToCv(cv);
            }
        }

        await _employeesRepository.AddOrUpdateCvInformation(employeeEntries);
        _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveCvData: Finished");
    }

}