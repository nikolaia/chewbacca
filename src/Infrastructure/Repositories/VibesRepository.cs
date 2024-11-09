using Infrastructure.ApiClients;
using Infrastructure.ApiClients.DTOs;

using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public interface IVibesRepository
{
    public Task<List<VibesEmploymentDTO>> GetEmployment();
    public Task<List<VibesConsultantDTO>> GetConsultants();
}

public class VibesRepository : IVibesRepository
{
    private readonly IVibesApiClient _vibesApiClient;
    private readonly ILogger<VibesRepository> _logger;

    public VibesRepository(IVibesApiClient vibesApiClient, ILogger<VibesRepository> logger)
    {
        _vibesApiClient = vibesApiClient;
        _logger = logger;
    }
    public async Task<List<VibesEmploymentDTO>> GetEmployment()
    {
        _logger.LogInformation("VibesRepository.GetEmployment: Fetching employee start and end dates from Vibes/Bemanning");
        
        var organisationResponse = await _vibesApiClient.GetOrganisations();

        var getEmploymentTasks = organisationResponse.Select(async organisationDto => 
            await _vibesApiClient.GetEmploymentDates(organisationDto.UrlKey));

        var apiResponses = await Task.WhenAll(getEmploymentTasks);

        return apiResponses.SelectMany(response => response).ToList();
    }

    public async Task<List<VibesConsultantDTO>> GetConsultants()
    {
        _logger.LogInformation("VibesRepository.GetConsultants: Fetching consultants from Vibes/Bemanning\"");

        IEnumerable<VibesOrganisationDTO> organisationsResponse = await _vibesApiClient.GetOrganisations();

        List<Task<IEnumerable<VibesConsultantDTO>>> getConsultantsTasks = organisationsResponse
            .Select(async organisationDto => await _vibesApiClient.GetConsultants(organisationDto.UrlKey)).ToList();

        IEnumerable<VibesConsultantDTO>[] apiResponses = await Task.WhenAll(getConsultantsTasks);

        return apiResponses.SelectMany(response => response).ToList();
    }
}