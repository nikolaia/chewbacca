using BlobStorage.Service;

using CvPartner.Models;
using CvPartner.Repositories;

using Employees.Models;
using Employees.Service;

namespace CvPartner.Service;

public class CvPartnerService
{
    private readonly CvPartnerRepository _cvPartnerRepository;
    private readonly EmployeesService _employeeService;
    private readonly BlobStorageService _blobStorageService;

    public CvPartnerService(CvPartnerRepository cvPartnerRepository, EmployeesService employeeService, BlobStorageService blobStorageService) {
        _cvPartnerRepository = cvPartnerRepository;
        _employeeService = employeeService;
        _blobStorageService = blobStorageService;
    }

    private static EmployeeEntity ConvertToEmployeeEntity(CVPartnerUserDTO dto) {
        return new EmployeeEntity {
            Name = dto.name,
            Email = dto.email,
            Telephone = dto.telephone,
            OfficeName = dto.office_name,
            ImageUrl = dto.image.url
        };
    }

    /**
     * <summary>Calls CvPartnerRepository's GetAllEmployee and converts them
     * to an employee. Adds to database.</summary>
     */
    public async Task GetCvPartnerEmployees()
    {
        var cvPartnerUserDTOs = await _cvPartnerRepository.GetAllEmployees();
        
        var employeeEntities = cvPartnerUserDTOs.Select(ConvertToEmployeeEntity);

        foreach (var employeeEntity in employeeEntities)
        {
            employeeEntity.ImageUrl = await _blobStorageService.UploadStream(employeeEntity.Name, employeeEntity.ImageUrl);
            await _employeeService.AddOrUpdateEmployee(employeeEntity);
        }
    }
}