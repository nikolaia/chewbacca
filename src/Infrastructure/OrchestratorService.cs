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

        foreach (var bemanning in bemanningEntries)
        {
            var cv = cvEntries.Find(cv => cv.email.ToLower().Trim() == bemanning.email.ToLower().Trim());

            // If the employee is not active or does not have a CV in CV Partner, ensure they do not exist in the database
            if (!IsActiveEmployee(bemanning) || cv == null)
            {
                _logger.LogInformation(
                    "Ensure employee with email {BemanningEmail} is deleted, since they are not active or do not have a CV in CV Partner",
                    bemanning.email);
                await EnsureEmployeeAndBlobIsDeleted(bemanning.email);
                continue;
            }

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
                    ImageThumbUrl =
                        cv.image.thumb.url != null
                            ? await _blobStorageService.SaveToBlob($"{cv.user_id}-thumb", cv.image.thumb.url)
                            : null,
                    OfficeName = cv.office_name,
                    StartDate = bemanning.startDate ?? new DateTime(2018, 08, 01),
                    EndDate = bemanning.endDate,
                    CountryCode = countryCode
                }
            });
        }

        // Delete all employees that are not in Bemanning
        var employees = await _employeesRepository.GetAllEmployees();
        var employeesNotInBemanning = employees.Where(employee =>
            bemanningEntries.All(bemanning => bemanning.email != employee.EmployeeInformation.Email)).ToList();

        foreach (var employee in employeesNotInBemanning)
        {
            _logger.LogInformation(
                "Deleting employee with email {EmployeeEmail} from database, since they are not in Bemanning",
                employee.EmployeeInformation.Email);
            await EnsureEmployeeAndBlobIsDeleted(employee.EmployeeInformation.Email);
        }

        _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveEmployeeData: Finished");
    }

    private async Task EnsureEmployeeAndBlobIsDeleted(string email)
    {
        var blobUrlToBeDeleted = await _employeesRepository.EnsureEmployeeIsDeleted(email);
        if (blobUrlToBeDeleted == null)
        {
            return;
        }

        _logger.LogInformation("Deleting blob with url {BlobUrlToBeDeleted}", blobUrlToBeDeleted);
        await _blobStorageService.DeleteBlob(blobUrlToBeDeleted);
    }

    /// <summary>
    /// Determines if the employee is active and should be added to the database
    /// </summary>
    /// <param name="employmentDto"></param>
    /// <returns></returns>
    private static bool IsActiveEmployee(VibesEmploymentDTO employmentDto)
    {
        if (employmentDto.startDate == null)
        {
            return false;
        }

        var startDateInPast = DateTime.Now >= employmentDto.startDate;
        var endDateNullOrInFuture = employmentDto.endDate == null || DateTime.Now <= employmentDto.endDate;

        return startDateInPast && endDateNullOrInFuture;
    }

    public async Task FetchMapAndSaveCvData()
    {
        _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveCVData: Started");
        var employeeEntries = await _employeesRepository.GetAllEmployees();
        var userEntries = await _cvPartnerRepository.GetAllEmployees();
        List<Cv> cvs = new();

        foreach (var user in employeeEntries.Select(employee => userEntries.Find(cv =>
                         cv.email.ToLower().Trim() == employee.EmployeeInformation.Email.ToLower().Trim()))
                     .Where(user => user != null))
        {
            var cvPartnerCv = await _cvPartnerRepository.GetEmployeeCv(user.user_id, user.default_cv_id);
            cvs.Add(CvDtoConverter.ToCv(cvPartnerCv));
        }

        await _employeesRepository.AddOrUpdateCvInformation(cvs);
        _logger.LogInformation("OrchestratorRepository: FetchMapAndSaveCvData: Finished");
    }
}