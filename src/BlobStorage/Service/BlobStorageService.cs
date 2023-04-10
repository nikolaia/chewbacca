using BlobStorage.Repositories;
namespace BlobStorage.Service;

public class BlobStorageService
{
    private readonly IBlobStorageRepository _blobStorageRepository;

    /**
     * <summary>Class for delegating tasks for blob storage</summary>
     * <param name="blobStorageRepository">Class for handling blob storage</param>
     */
    public BlobStorageService(IBlobStorageRepository blobStorageRepository)
    {
        _blobStorageRepository = blobStorageRepository;
    }

    /**
     * <summary> Delegates a task to <see cref="BlobStorageRepository"/> to save employee image to blob storage</summary>
     */
    public async Task<string> SaveToBlob(string employeeName, string employeeImageUri, DateTime updatedAt)
    {
        return await _blobStorageRepository.SaveToBlob(employeeName, employeeImageUri, updatedAt);
    }
}