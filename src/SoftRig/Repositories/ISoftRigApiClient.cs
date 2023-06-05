using SoftRig.Models;

using Refit;

namespace SoftRig.Repositories;

public interface ISoftRigApiClient
{

    [Get("/gadgetEntries")]
    Task<IApiResponse<IEnumerable<GadgetJournalEntry>>> GetAllGadgetJournalEntries();
}