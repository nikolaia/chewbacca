using Refit;
using Shared;
using Microsoft.Extensions.Options;
using CvPartner.DTOs;

namespace CvPartner;

public class CvPartnerRepository
{
    private readonly IOptionsSnapshot<AppSettings> _appSettings;

    public CvPartnerRepository(IOptionsSnapshot<AppSettings> appSettings)
    {
        _appSettings = appSettings;
    }

    public async Task<IEnumerable<CVPartnerUserDTO>> GetAllEmployees()
    {
        var cvPartnerApi = RestService.For<IEmployeeApi>("https://variant.cvpartner.com/api/v1");

        var employees = await cvPartnerApi.GetAllEmployee(_appSettings.Value.Token);
        return employees;
    }

    public interface IEmployeeApi
    {
        [Get("/users")]
        Task<IEnumerable<CVPartnerUserDTO>> GetAllEmployee([Authorize("Token")] string authorization);
    }
}