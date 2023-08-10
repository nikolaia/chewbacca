using ApplicationCore.Models;

namespace ApplicationCore.Interfaces;

public interface IEmergencyContactRepository
{
    Task<EmergencyContact?> GetByEmployee(string alias, string country);

    Task<bool> AddOrUpdateEmergencyContact(string alias, string country,
        EmergencyContact emergencyContact);
}