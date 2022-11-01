using CvPartner.Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared;
using EmployeeModel = Employee.Models.Employee;
using CvPartner;

namespace CvPartnerService;

public class CvPartnerService
{
    private readonly CVPartnerRepository _cvPartnerRepository;

    public CvPartnerService(CVPartnerRepository _cvPartnerRepository) {
        this._cvPartnerRepository = _cvPartnerRepository;
    }

    public async Task<IEnumerable<EmployeeModel>> FormatData ()
    {
        var cvPartnerDto = this._cvPartnerRepository.GetAllEmployees();
        return cvPartnerDto.Select(person => new EmployeeModel
        {
            Name = person.name,
            FullName = person.name,
            Email = person.email,
            Telephone = person.telephone,
            OfficeName = person.office_name,
            ImageUrl = person.image.url
        })
        .ToList();
    }
}