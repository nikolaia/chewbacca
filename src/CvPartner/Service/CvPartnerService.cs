using Bemanning;

using CvPartner.Models;
using CvPartner.Repositories;

using Employees.Models;
using Employees.Service;

namespace CvPartner.Service;

public class CvPartnerService
{
    private readonly CvPartnerRepository _cvPartnerRepository;
    private readonly EmployeesService _employeeService;
    private readonly BemanningRepository _bemanningRepository;

    public CvPartnerService(CvPartnerRepository _cvPartnerRepository, EmployeesService _employeeService, BemanningRepository bemanningRepository)
    {
        this._cvPartnerRepository = _cvPartnerRepository;
        this._employeeService = _employeeService;
        _bemanningRepository = bemanningRepository;
    }

    private static EmployeeEntity ConvertToEmployeeEntity(CVPartnerUserDTO dto, DateTime startDate)
    {
        EmployeeEntity employee = new()
        {
            Name = dto.name,
            Email = dto.email,
            Telephone = dto.telephone,
            OfficeName = dto.office_name,
            ImageUrl = dto.image.url,
            StartDate = startDate
        };
        return employee;
    }

    /**
     * <summary>Calls CvPartnerRepository's GetAllEmployee and converts them
     * to an employee. Adds to database.</summary>
     */
    public async Task GetCvPartnerEmployees()
    {
        var cvPartnerUserDTOs = await _cvPartnerRepository.GetAllEmployees();
        List<BemanningEmployee> bemanningEmployeeDTO = await _bemanningRepository.GetBemanningDataForEmployees();
        // var employeeEntities = cvPartnerUserDTOs.Select(ConvertToEmployeeEntity);

        foreach (CVPartnerUserDTO cvPartnerUser in cvPartnerUserDTOs)
        {
            BemanningEmployee? employeeStartDate = bemanningEmployeeDTO.Find(e => e.Email == cvPartnerUser.email);
            if (employeeStartDate != null)
            {
                EmployeeEntity convertedEmployee = ConvertToEmployeeEntity(cvPartnerUser, employeeStartDate.StartDate);
                await _employeeService.AddOrUpdateEmployee(convertedEmployee);
            }
        }
    }
}