using SoftRig.Models;
using SoftRig.Service;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

[ApiController]
[Route("[controller]")]
public class SoftRigController : ControllerBase
{
    private readonly SoftRigService _softRigService;

    public SoftRigController(SoftRigService softRigService)
    {
        this._softRigService = softRigService;
    }

    [HttpGet("/softrig/gadgetEntries")]
    [OutputCache(Duration = 60)]
    public async Task<List<GadgetJournalEntry>> GetAllGadgetEntries()
    {
        var token = await _softRigService.RequestTokenAsync();
        var gadgetEntries = await _softRigService.GetGadgetJournalEntries(token.AccessToken!);

        // TODO
        return null;
    }
}