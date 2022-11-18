using CvPartner.Models;
using CvPartner.Service;

using Microsoft.AspNetCore.Mvc;

namespace CvPartner.Api;

[ApiController]
[Route("[controller]")]
public class CvPartnerController : ControllerBase
{
    private readonly CvPartnerService _cvPartnerService;

    public CvPartnerController(CvPartnerService cvPartnerService)
    {
        this._cvPartnerService = cvPartnerService;
    }
    
    [HttpGet]
    public async Task<IEnumerable<CVPartnerUserDTO>> Get()
    {
        return await _cvPartnerService.GetCvPartnerEmployees();
    }
}