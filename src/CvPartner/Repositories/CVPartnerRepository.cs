using CvPartner.Models;

using Microsoft.Extensions.Options;

using Shared;

namespace CvPartner.Repositories;

public class CvPartnerRepository
{
    private readonly IOptionsSnapshot<AppSettings> _appSettings;
    private readonly IEmployeeApi _employeeApi;

    public CvPartnerRepository(IOptionsSnapshot<AppSettings> appSettings, IEmployeeApi _employeeApi)
    {
        _appSettings = appSettings;
        this._employeeApi = _employeeApi;
    }

    public async Task<IEnumerable<CVPartnerUserDTO>> GetAllEmployees()
    {
        return await _employeeApi.GetAllEmployee(_appSettings.Value.Token);
    }
}