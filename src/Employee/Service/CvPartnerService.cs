using CvPartner.Api;
using CvPartner.DTOs;

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

        var ret = new List<EmployeeModel>();
        
        foreach (var person in cvPartnerDto)
        {
            var emp = new EmployeeModel();

            emp.Name = person.name;
            emp.FullName = person.name;
            emp.Email = person.email;
            emp.Telephone = person.telephone;
            emp.OfficeName = person.office_name;
            emp.ImageUrl = person.image.url;
            
            ret.Add(emp);
        }

        return ret;
    }






}