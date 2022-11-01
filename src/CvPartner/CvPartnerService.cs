using CvPartner.Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared;
using Employees.Models;
using CvPartner;
using Employees.Service;
using CvPartner.DTOs;

namespace CvPartner.Service;

public class CvPartnerService
{
    private readonly CVPartnerRepository _cvPartnerRepository;
    private readonly EmployeesService _employeeService;

    public CvPartnerService(CVPartnerRepository _cvPartnerRepository, EmployeesService _employeeService) {
        this._cvPartnerRepository = _cvPartnerRepository;
        this._employeeService = _employeeService;
    }

    private Employee ConvertToEmployee(CVPartnerUserDTO dto) {
        return new Employee {
            Name = dto.name,
            FullName = dto.name,
            Email = dto.email,
            Telephone = dto.telephone,
            OfficeName = dto.office_name,
            ImageUrl = dto.image.url
        };
    }

    public async Task GetCVPartnerEmployees(){
        // Hent data
        // konverter
        // sendt til lagring
        var cvpartnerEmployees = await _cvPartnerRepository.GetAllEmployees();

        cvpartnerEmployees
            .Select(cvpartnerEmployee => ConvertToEmployee(cvpartnerEmployee))
            .Select(async (employee) => await _employeeService.AddOrUpdateEmployees(employee));
            // TODO Må sikkert ha en spesiell syntaks for å kjøre async/await på en liste
    }

}