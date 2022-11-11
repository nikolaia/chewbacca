using CvPartner.Models;
using CvPartner.Repositories;

using Employees.Models;
using Employees.Service;

namespace CvPartner.Service;

public class CvPartnerService
{
    private readonly CvPartnerRepository _cvPartnerRepository;
    private readonly EmployeesService _employeeService;

    public CvPartnerService(CvPartnerRepository _cvPartnerRepository, EmployeesService _employeeService) {
        this._cvPartnerRepository = _cvPartnerRepository;
        this._employeeService = _employeeService;
    }

    private static EmployeeEntity ConvertToEmployeeEntity(CVPartnerUserDTO dto) {
        return new EmployeeEntity {
            Name = dto.name,
            FullName = dto.name,
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
        
        var saveToDbTasks = cvPartnerUserDTOs
            .Select(ConvertToEmployeeEntity)
            .Select(_employeeService.AddOrUpdateEmployee);
        
        await Task.WhenAll(saveToDbTasks);
    }
}