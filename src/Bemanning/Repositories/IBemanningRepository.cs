namespace Bemanning;

public interface IBemanningRepository
{
    Task<List<BemanningEmployee>> GetBemanningDataForEmployees();
}