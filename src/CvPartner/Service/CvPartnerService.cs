using CvPartner.Models;
using CvPartner.Repositories;

namespace CvPartner.Service;

public class CvPartnerService
{
    private readonly CvPartnerRepository _cvPartnerRepository;

    public CvPartnerService(CvPartnerRepository _cvPartnerRepository)
    {
        this._cvPartnerRepository = _cvPartnerRepository;
    }

    /**
     * <summary>Calls CvPartnerRepository's GetAllEmployee and converts them
     * to an employee. Adds to database.</summary>
     */
    public async Task<IEnumerable<CVPartnerUserDTO>> GetCvPartnerEmployees()
    {
        return await _cvPartnerRepository.GetAllEmployees();
    }
}