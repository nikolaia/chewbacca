using CvPartner.Models;

using Refit;

namespace CvPartner.Repositories;

public interface ICvPartnerApiClient
{
    [Get("/users/search?size=200&deactivated=false")]
    Task<IApiResponse<IEnumerable<CVPartnerUserDTO>>> GetAllEmployee([Authorize("Token")] string authorization);
}