using CvPartner.Models;

using Microsoft.Extensions.Options;

using Shared;

namespace CvPartner.Repositories;

public class CvPartnerRepository
{
    private readonly IOptionsSnapshot<AppSettings> _appSettings;
    private readonly ICvPartnerApiClient _cvPartnerApiClient;

    public CvPartnerRepository(IOptionsSnapshot<AppSettings> appSettings, ICvPartnerApiClient cvPartnerApiClient)
    {
        _appSettings = appSettings;
        this._cvPartnerApiClient = cvPartnerApiClient;
    }

    public async Task<IEnumerable<CVPartnerUserDTO>> GetAllEmployees()
    {
        return await _cvPartnerApiClient.GetAllEmployee(_appSettings.Value.CvPartner.Token);
    }
}