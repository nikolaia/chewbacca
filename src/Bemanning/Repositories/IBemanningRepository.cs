namespace Bemanning.Repositories;

public interface IBemanningRepository
{
    Task<List<BemanningEmployee>> GetBemanningDataForEmployees();
}