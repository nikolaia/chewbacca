using Microsoft.AspNetCore.Mvc;

using Orchestrator.Service;

namespace Orchestrator.Api;

[ApiController]
[Route("[controller]")]
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
}