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

        foreach (var bemanning in bemanningEntries)
        {
            var cv = cvEntries.Find(cv => cv.email.ToLower().Trim() == bemanning.Email.ToLower().Trim());

            if (cv is { image.url: not null })
            {
                var defaultPhoneNumberRegion = cv.email.ToLower().EndsWith(".se") ? "SE" : "NO";

                var phoneNumber = phoneNumberUtil.IsPossibleNumber(cv.telephone, defaultPhoneNumberRegion)
                    ? phoneNumberUtil.Format(phoneNumberUtil.Parse(cv.telephone, defaultPhoneNumberRegion),
                        PhoneNumbers.PhoneNumberFormat.E164)
                    : null;

                await _employeesService.AddOrUpdateEmployee(new EmployeeEntity
                {
                    Name = cv.name,
                    Email = cv.email,
                    Telephone = phoneNumber,
                    ImageUrl = await _blobStorageService.SaveToBlob(cv.user_id, cv.image.url),
                    OfficeName = cv.office_name,
                    StartDate = bemanning.StartDate
                });
            }
            else
            {
                // TODO: Fetch the image url from the database if the employee exists, and delete the image from blob storage if it exists.
                await _employeesService.EnsureEmployeeIsDeleted(bemanning.Email);
            }
        }

        _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveEmployeeData: Finished");
    }
}