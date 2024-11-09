using Infrastructure.ApiClients.DTOs;

using Refit;

namespace Infrastructure.ApiClients;

[Headers("Authorization: Bearer")]
public interface IVibesApiClient
{
    [Get("/v0/organisations")]
    Task<IEnumerable<VibesOrganisationDTO>> GetOrganisations();
    
    [Get("/v0/{companyCountry}/consultants/employment")]
    Task<IEnumerable<VibesEmploymentDTO>> GetEmploymentDates(string companyCountry);

    [Get("/v0/{companyCountry}/consultants")]
    Task<IEnumerable<VibesConsultantDTO>> GetConsultants(string companyCountry);
}
