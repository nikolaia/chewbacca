using CvPartner.Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared;
using EmployeeModel = Employee.Models.Employee;

namespace CvPartnerService;

public class CvPartnerService
{
    private readonly IOptionsSnapshot<AppSettings> _appSettings;
    private readonly ILogger<GetAllEmployees> _getAllEmployeesLogger;

    public CvPartnerService(IOptionsSnapshot<AppSettings> appSettings,
        ILogger<GetAllEmployees> getAllEmployeesLogger)
    {
        _appSettings = appSettings;
        _getAllEmployeesLogger = getAllEmployeesLogger;
    }
    
    public async Task<IEnumerable<EmployeeModel>> FormatData ()
    {
        var cvPartnerDto = await new GetAllEmployees(_appSettings, _getAllEmployeesLogger).Get();

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