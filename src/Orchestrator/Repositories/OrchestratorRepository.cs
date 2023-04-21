using Bemanning;

using BlobStorage.Service;

using CvPartner.Models;
using CvPartner.Service;

using Employees.Models;
using Employees.Service;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Shared;

namespace Orchestrator.Repositories;

public class OrchestratorRepository
{
    private readonly EmployeesService _employeesService;
    private readonly CvPartnerService _cvPartnerService;
    private readonly IBemanningRepository _bemanningRepository;
    private readonly BlobStorageService _blobStorageService;
    private readonly ILogger<OrchestratorRepository> _logger;

    public OrchestratorRepository(EmployeesService employeesService, CvPartnerService cvPartnerService,
        IBemanningRepository bemanningRepository,
        BlobStorageService blobStorageService,
        ILogger<OrchestratorRepository> logger)
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

        foreach (var bemanning in bemanningEntries)
        {
            var cv = cvEntries.Find(cv => cv.email.ToLower().Trim() == bemanning.Email.ToLower().Trim());

            if (cv is { image.url: not null })
            {
                // TODO: Parse and validate phone number
                await _employeesService.AddOrUpdateEmployee(new EmployeeEntity
                {
                    Name = cv.name,
                    Email = cv.email,
                    Telephone = cv.telephone,
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