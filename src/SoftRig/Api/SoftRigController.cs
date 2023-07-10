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

    // [HttpGet("/softrig/gadgetEntries")]
    // [OutputCache(Duration = 60)]
    // public async Task<List<GadgetJournalEntry>> GetAllGadgetEntries()
    // {
    //     var token = await _softRigService.RequestTokenAsync();
    //     var gadgetEntries = await _softRigService.GetGadgetJournalEntries(token.AccessToken!);

    //     // TODO
    //     return null;
    // }


    // [HttpPost("/softrig/gadgetEntries")]
    // [OutputCache(Duration = 60)]
    // public async Task<List<GadgetJournalEntry>> GetAllGadgetEntries()
    // {
    //     var token = await _softRigService.RequestTokenAsync();
    //     var gadgetEntries = await _softRigService.GetGadgetJournalEntries(token.AccessToken!);

    //     // TODO
    //     return null;
    // }


    [HttpGet("/softrig/companyKey")]
    [OutputCache(Duration = 60)]
    public async Task<string> GetCompanyKey()
    {
        var token = await _softRigService.RequestTokenAsync();
        return await _softRigService.GetCompanyKey(token.AccessToken!, "Variant");
    }

    // TODO: just during development
    [HttpGet("/softrig/employee/update")]
    [OutputCache(Duration = 60)]
    public async Task<SoftRigEmployee> TestUpdateEmployee([FromQuery] string email)
    {
        var token = await _softRigService.RequestTokenAsync();
        var employee = await _softRigService.GetSoftRigEmployee(token.AccessToken!, email);
        var testInformation = new SoftRigEmployeeDto
        {
            Phone = "11223344",
            AccountNumber = "12341234123",
            Address = "En annen adresse",
            ZipCode = "1234",
            City = "Sarpsborg"
        };

        var updateSuccess = await _softRigService.UpdateEmployee(email, testInformation);

        return await _softRigService.GetSoftRigEmployee(token.AccessToken!, email);
    }

    // TODO: just during development
    [HttpGet("/softrig/employee")]
    [OutputCache(Duration = 60)]
    public async Task<SoftRigEmployee> GetEmployee([FromQuery] string email)
    {
        var token = await _softRigService.RequestTokenAsync();
        return await _softRigService.GetSoftRigEmployee(token.AccessToken!, email);
    }

    // TODO: just during development
    [HttpGet("/softrig/employees")]
    [OutputCache(Duration = 60)]
    public async Task<List<SoftRigEmployee>> GetAllEmployees()
    {
        var token = await _softRigService.RequestTokenAsync();
        return await _softRigService.GetSoftRigEmployees(token.AccessToken!);
    }
}