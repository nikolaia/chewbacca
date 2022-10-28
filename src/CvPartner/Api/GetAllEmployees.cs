using CvPartner.DTOs;
using CvPartner.Utils;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Refit;

using Shared;

namespace CvPartner.Api;

[ApiController]
[Route("[controller]")]
public class GetAllEmployees : ControllerBase
{
    private readonly IOptionsSnapshot<AppSettings> _appSettings;
    private readonly ILogger<GetAllEmployees> _logger;


    public GetAllEmployees(IOptionsSnapshot<AppSettings> appSettings, ILogger<GetAllEmployees> logger)
    {
        _appSettings = appSettings;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IEnumerable<CVPartnerUserDTO>> Get()
    {
        _logger.LogInformation("Getting employees from database");
        return await new CVPartnerRepository(_appSettings).GetCVPartnerDTO();
    }
}