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
     * <summary>Calls CvPartnerRepository's GetAllEmployee and converts them
     * to an employee. Adds to database.</summary>
     */
    public async Task<List<CVPartnerUserDTO>> GetCvPartnerEmployees()
    {
        return await _cvPartnerRepository.GetAllEmployees();
    }

    /**
     * <summary>Calls CvPartnerRepository's GetAllEmployee and converts them
     * to an employee. Adds to database.</summary>
     */
    public async Task<List<CvPartnerPresentationDTO>> GetCvPartnerPresentations(string userId, string cvId)
    {
        var employee = await _cvPartnerRepository.GetEmployeeCv(userId, cvId);
        return employee.presentations ?? new List<CvPartnerPresentationDTO>();
    }
}