using CvPartner.Models;
using CvPartner.Repositories;

namespace CvPartner.Service;

public class CvPartnerService
{
    private readonly CvPartnerRepository _cvPartnerRepository;

    public CvPartnerService(CvPartnerRepository cvPartnerRepository)
    {
        _cvPartnerRepository = cvPartnerRepository;
    }
    
    /**
     * <summary> Calls CvPartnerRepository's GetAllEmployee and converts them
     to an employee. Adds to database.</summary>
     */
    public async Task<List<CVPartnerUserDTO>> GetCvPartnerEmployees()
    {
        return await _cvPartnerRepository.GetAllEmployees();
    }

    public async Task<CVPartnerCvDTO> GetCvForEmployee(string userId, string cvId)
    {
        return await _cvPartnerRepository.GetEmployeeCv(userId, cvId);
    }
}