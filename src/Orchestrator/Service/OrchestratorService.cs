using Orchestrator.Repositories;

namespace Orchestrator.Service;

public class OrchastratorService
{
    private readonly OrchastratorRepository _orchastratorRepository;

    public OrchastratorService(OrchastratorRepository orchastratorRepository)
    {
        _orchastratorRepository = orchastratorRepository;
    }

    public async Task AddEmployeeToDatabase()
    {
        await _orchastratorRepository.PostEmployee();
    }
}