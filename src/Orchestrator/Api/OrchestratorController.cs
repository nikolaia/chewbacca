using Microsoft.AspNetCore.Mvc;

using Orchestrator.Service;

namespace Orchestrator.Api;

[ApiController]
[Route("[controller]")]
public class OrchestratorController : ControllerBase
{
    private readonly OrchastratorService _orchastratorService;

    public OrchestratorController(OrchastratorService orchastratorService)
    {
        _orchastratorService = orchastratorService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await _orchastratorService.AddEmployeeToDatabase();
        return Ok();
    }
}