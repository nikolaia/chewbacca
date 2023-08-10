using ApplicationCore.Models;

namespace ApplicationCore.Interfaces;

public interface IBemanningRepository
{
    Task<List<BemanningEmployee>> GetBemanningDataForEmployees();
}