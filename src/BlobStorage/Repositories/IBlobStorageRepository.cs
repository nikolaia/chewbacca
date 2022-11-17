namespace BlobStorage.Repositories;

public interface IBlobStorageRepository
{
    public Task<string> SaveToBlob(string employeeName, string employeeImageUri);
}