using Infrastructure.ApiClients.DTOs;

using Refit;

namespace Infrastructure.ApiClients;

public interface ICvPartnerApiClient
{
    [Get("/v2/users/search?size=200&deactivated=false")]
    Task<IApiResponse<IEnumerable<CVPartnerUserDTO>>> GetAllEmployee([Authorize("Token")] string authorization);

    [Get("/v3/cvs/{userId}/{cvId}")]
    Task<IApiResponse<CVPartnerCvDTO>> GetEmployeeCv(string userId, string cvId, [Authorize("Token")] string authorization);
}