using CvPartner.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;
using Shared;

namespace CvPartner.Api;

[ApiController]
[Route("[controller]")]
public class DummyController : ControllerBase
{
    private readonly CVPartnerRepository _cvPartnerRepository;

    public DummyController(CVPartnerRepository _cvPartnerRepository)
    {
        this._cvPartnerRepository = _cvPartnerRepository;
    }
    
    [HttpGet]
    public async Task<IEnumerable<CVPartnerUserDTO>> Get()
    {
        return await _cvPartnerRepository.GetAllEmployees();
    }
}