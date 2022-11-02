using CvPartner.DTOs;
using Employees.Models;
using Employees.Service;

namespace CvPartner;

public class CvPartnerService
{
    private readonly CvPartnerRepository _cvPartnerRepository;
    private readonly EmployeesService _employeeService;

    public CvPartnerService(CvPartnerRepository _cvPartnerRepository, EmployeesService _employeeService) {
        this._cvPartnerRepository = _cvPartnerRepository;
        this._employeeService = _employeeService;
    }

    private static Employee ConvertToEmployee(CVPartnerUserDTO dto) {
        return new Employee {
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
    public async Task GetCvPartnerEmployees(){
        var cvpartnerEmployees = await _cvPartnerRepository.GetAllEmployees();

        cvpartnerEmployees
            .Select(cvpartnerEmployee => ConvertToEmployee(cvpartnerEmployee))
            .Select(async (employee) => await _employeeService.AddOrUpdateEmployees(employee));
    }
}