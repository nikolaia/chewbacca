using CvPartner.Models;

using Refit;

namespace CvPartner.Repositories;

public interface IEmployeeApi
{
    [Get("/users")]
    Task<IEnumerable<CVPartnerUserDTO>> GetAllEmployee([Authorize("Token")] string authorization);
}