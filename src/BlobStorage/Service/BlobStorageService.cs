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
    public Task<string?> SaveToBlob(string employeeName, string employeeImageUri)
    {
        return _blobStorageRepository.SaveToBlob(employeeName, employeeImageUri);
    }

    public async Task DeleteBlob(string blobUrlToBeDeleted)
    {
        await _blobStorageRepository.DeleteBlob(blobUrlToBeDeleted);
    }
}