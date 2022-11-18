using Bemanning;

using BlobStorage.Service;

using CvPartner.Models;
using CvPartner.Service;

using Employees.Models;
using Employees.Service;

namespace Orchestrator.Repositories;

public class OrchestratorRepository
{
    private readonly EmployeesService _employeesService;
    private readonly CvPartnerService _cvPartnerService;
    private readonly IBemanningRepository _bemanningRepository;
    private readonly BlobStorageService _blobStorageService;

    public OrchestratorRepository(EmployeesService employeesService, CvPartnerService cvPartnerService, IBemanningRepository bemanningRepository,
        BlobStorageService blobStorageService)
    {
        _employeesService = employeesService;
        _cvPartnerService = cvPartnerService;
        _bemanningRepository = bemanningRepository;
        _blobStorageService = blobStorageService;
    }

    public async Task FetchMapAndSaveEmployeeData()
    {
        var cvPartnerDTOs = await _cvPartnerService.GetCvPartnerEmployees();
        var BemanningDTOs = await _bemanningRepository.GetBemanningDataForEmployees();
        foreach (CVPartnerUserDTO cvPartnerDTO in cvPartnerDTOs)
        {
            cvPartnerDTO.image.url = await _blobStorageService.UploadStream(cvPartnerDTO.name, cvPartnerDTO.image.url);

            var matchingEmployee = BemanningDTOs.Find(e => e.Email == cvPartnerDTO.email);
            {
                if (matchingEmployee != null)
                {
                    var employee = ConvertToEmployeeEntityWithStartDate(cvPartnerDTO, matchingEmployee.StartDate);
                    await _employeesService.AddOrUpdateEmployee(employee);
                }
                else
                {
                    var employee = ConvertToEmployeeEntity(cvPartnerDTO);
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