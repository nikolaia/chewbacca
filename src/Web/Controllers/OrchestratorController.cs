using Infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class OrchestratorController : ControllerBase
{
    private readonly OrchestratorService _orchestratorService;

    public OrchestratorController(OrchestratorService orchestratorService)
    {
        _orchestratorService = orchestratorService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await _orchestratorService.FetchMapAndSaveEmployeeData();
        return Ok();
    }

    [HttpGet("cv")]
    public async Task<IActionResult> GetCvs()
    {
        await _orchestratorService.FetchMapAndSaveCvData();
        return Ok();
    }
}