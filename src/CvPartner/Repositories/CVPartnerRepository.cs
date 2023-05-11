using CvPartner.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Shared;

namespace CvPartner.Repositories;

public class CvPartnerRepository
{
    private readonly IOptionsSnapshot<AppSettings> _appSettings;
    private readonly ICvPartnerApiClient _cvPartnerApiClient;
    private readonly ILogger<CvPartnerRepository> _logger;

    public CvPartnerRepository(IOptionsSnapshot<AppSettings> appSettings, ICvPartnerApiClient cvPartnerApiClient, ILogger<CvPartnerRepository> logger)
    {
        _appSettings = appSettings;
        _cvPartnerApiClient = cvPartnerApiClient;
        _logger = logger;
    }

    public async Task<List<CVPartnerUserDTO>> GetAllEmployees()
    {
        var apiResponse = await _cvPartnerApiClient.GetAllEmployee(_appSettings.Value.CvPartner.Token);
        
        if (apiResponse is {IsSuccessStatusCode: true, Content: not null })
        {
            return apiResponse.Content.ToList();
        }
        
        _logger.LogCritical(apiResponse.Error, "Exception when calling CVPartner");
        return new List<CVPartnerUserDTO>();
    }
}