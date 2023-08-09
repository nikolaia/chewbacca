using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces;

public interface IBemanningRepository
{
    Task<List<BemanningEmployee>> GetBemanningDataForEmployees();
}