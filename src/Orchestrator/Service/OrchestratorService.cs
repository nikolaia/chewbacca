using Orchestrator.Repositories;

namespace Orchestrator.Service;

public class OrchestratorService
{
    private readonly OrchestratorRepository _orchestratorRepository;

    public OrchestratorService(OrchestratorRepository orchestratorRepository)
    {
        _orchestratorRepository = orchestratorRepository;
    }

    public async Task AddEmployeeToDatabase()
    {
        await _orchestratorRepository.FetchMapAndSaveEmployeeData();
    }
}