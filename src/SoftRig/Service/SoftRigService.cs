using SoftRig.Models;
using SoftRig.Repositories;

namespace SoftRig.Service;

public class SoftRigService
{
    private readonly SoftRigRepository _softRigRepository;

    public SoftRigService(SoftRigRepository softRigRepository)
    {
        _softRigRepository = softRigRepository;
    }

    public async Task<IdentityModel.Client.TokenResponse> RequestTokenAsync()
    {
        return await _softRigRepository.RequestTokenAsync();
    }

    public async Task<List<GadgetJournalEntry>> GetGadgetJournalEntries(string token)
    {
        return await _softRigRepository.GetAllGadgetEntries(token);
    }
}