using Bemanning;

using BlobStorage.Service;

using CvPartner.Service;

using Employees.Models;
using Employees.Service;

using Microsoft.Extensions.Logging;

namespace Orchestrator.Service;

public class OrchestratorService
{
    private readonly EmployeesService _employeesService;
    private readonly CvPartnerService _cvPartnerService;
    private readonly IBemanningRepository _bemanningRepository;
    private readonly BlobStorageService _blobStorageService;
    private readonly ILogger<OrchestratorService> _logger;

    public OrchestratorService(EmployeesService employeesService, CvPartnerService cvPartnerService,
        IBemanningRepository bemanningRepository,
        BlobStorageService blobStorageService,
        ILogger<OrchestratorService> logger)
    {
        _employeesService = employeesService;
        _cvPartnerService = cvPartnerService;
        _bemanningRepository = bemanningRepository;
        _blobStorageService = blobStorageService;
        _logger = logger;
    }

    public async Task FetchMapAndSaveEmployeeData()
    {
        _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveEmployeeData: Started");
        var bemanningEntries = await _bemanningRepository.GetBemanningDataForEmployees();
        var cvEntries = await _cvPartnerService.GetCvPartnerEmployees();

        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

        foreach (var bemanning in bemanningEntries.Where(IsActiveEmployee))
        {
            var cv = cvEntries.Find(cv => cv.email.ToLower().Trim() == bemanning.Email.ToLower().Trim());

            if (cv is { image.url: not null })
            {
                var countryCode = cv.email.ToLower().EndsWith(".se") ? "SE" : "NO";

                var phoneNumber = phoneNumberUtil.IsPossibleNumber(cv.telephone, countryCode)
                    ? phoneNumberUtil.Format(phoneNumberUtil.Parse(cv.telephone, countryCode),
                        PhoneNumbers.PhoneNumberFormat.E164)
                    : null;

                await _employeesService.AddOrUpdateEmployee(new EmployeeEntity
                {
                    Name = cv.name,
                    Email = cv.email,
                    Telephone = phoneNumber,
                    ImageUrl = await _blobStorageService.SaveToBlob(cv.user_id, cv.image.url),
                    OfficeName = cv.office_name,
                    StartDate = bemanning.StartDate,
                    EndDate = bemanning.EndDate,
                    CountryCode = countryCode.ToLower()
                });
            }
            else
            {
                // If the employee does not exist in CV Partner, only in Bemanning, we should ensure the employee is not in the database.
                _logger.LogInformation(
                    "Deleting employee with email {BemanningEmail} from database, since it does not exist in CV Partner",
                    bemanning.Email);
                var blobUrlToBeDeleted = await _employeesService.EnsureEmployeeIsDeleted(bemanning.Email);
                if (blobUrlToBeDeleted == null)
                {
                    continue;
                }

                _logger.LogInformation("Deleting blob with url {BlobUrlToBeDeleted}", blobUrlToBeDeleted);
                await _blobStorageService.DeleteBlob(blobUrlToBeDeleted);
            }
        }

        var blobUrlsToBeDeleted = await _employeesService.EnsureEmployeesWithEndDateBeforeTodayAreDeleted();
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
}