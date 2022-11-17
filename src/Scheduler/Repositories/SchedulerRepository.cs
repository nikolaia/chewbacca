using Bemanning;

using CvPartner.Models;
using CvPartner.Service;

using Employees.Models;
using Employees.Service;

namespace Scheduler.Repositories;

public class SchedulerRepository
{
    private readonly EmployeesService _employeesService;
    private readonly CvPartnerService _cvPartnerService;
    private readonly BemanningRepository _bemanningRepository;

    public SchedulerRepository(EmployeesService employeesService, CvPartnerService cvPartnerService, BemanningRepository bemanningRepository)
    {
        _employeesService = employeesService;
        _cvPartnerService = cvPartnerService;
        _bemanningRepository = bemanningRepository;
    }

    public async Task postEmployee()
    {
        IEnumerable<CVPartnerUserDTO> cvPartnerDTOs = await _cvPartnerService.GetCvPartnerEmployees();
        List<BemanningEmployee> BemanningDTOs = await _bemanningRepository.GetBemanningDataForEmployees();
        foreach (CVPartnerUserDTO cvPartnerDTO in cvPartnerDTOs)
        {
            BemanningEmployee? matchingEmployee = BemanningDTOs.Find(e => e.Email == cvPartnerDTO.email);
            {
                if (matchingEmployee != null)
                {
                    EmployeeEntity employee = ConvertToEmployeeEntityWithStartDate(cvPartnerDTO, matchingEmployee.StartDate);
                    await _employeesService.AddOrUpdateEmployee(employee);
                }
                else
                {
                    EmployeeEntity employee = ConvertToEmployeeEntity(cvPartnerDTO);
                    await _employeesService.AddOrUpdateEmployee(employee);
                }
            }
        }
    }

    private EmployeeEntity ConvertToEmployeeEntityWithStartDate(CVPartnerUserDTO cvPartnerUserDto, DateTime startDate)
    {
        return new EmployeeEntity
        {
            Name = cvPartnerUserDto.name,
            Email = cvPartnerUserDto.email,
            Telephone = cvPartnerUserDto.telephone,
            ImageUrl = cvPartnerUserDto.image.url,
            OfficeName = cvPartnerUserDto.office_name,
            StartDate = startDate
        };
    }

    private EmployeeEntity ConvertToEmployeeEntity(CVPartnerUserDTO cvPartnerUserDto)
    {
        return new EmployeeEntity
        {
            Name = cvPartnerUserDto.name,
            Email = cvPartnerUserDto.email,
            Telephone = cvPartnerUserDto.telephone,
            ImageUrl = cvPartnerUserDto.image.url,
            OfficeName = cvPartnerUserDto.office_name
        };
    }
}