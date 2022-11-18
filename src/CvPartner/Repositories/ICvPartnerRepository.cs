using CvPartner.Models;

namespace CvPartner.Repositories;

public interface ICvPartnerRepository
{
    Task<IEnumerable<CVPartnerUserDTO>> GetAllEmployees();
}