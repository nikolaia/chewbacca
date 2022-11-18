namespace Bemanning;

public interface IBemanningReository
{
    Task<List<BemanningEmployee>> GetBemanningDataForEmployees();
}