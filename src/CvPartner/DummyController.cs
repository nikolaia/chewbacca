using Microsoft.AspNetCore.Mvc;

namespace CvPartner.Api;

[ApiController]
[Route("[controller]")]
public class DummyController : ControllerBase
{
    private readonly CvPartnerService _cvPartnerService;

    public DummyController(CvPartnerService _cvPartnerService)
    {
        this._cvPartnerService = _cvPartnerService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await _cvPartnerService.GetCvPartnerEmployees();
        return Ok();
    }
}