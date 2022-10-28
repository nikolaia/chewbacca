using CvPartner.DTOs;

using Refit;

namespace CvPartner.Utils;

public class Interfaces
{
    public interface IEmployeeApi
    {
        [Get("/users?")]
        Task<IEnumerable<CVPartnerUserDTO>> GetAllEmployee([Authorize("Token")] string authorization);
    }
}