using BlobStorage.Repositories;

namespace BlobStorage.Service;

public class BlobStorageService
{
    private static IBlobStorageRepository _blobStorageRepository;
    
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
    public async Task<string> UploadStream(string employeeName, string employeeImageUri)
    {
        if (employeeImageUri != null)
        {
            return await _blobStorageRepository.SaveToBlob(employeeName, employeeImageUri);
        }
        else return "";
    }
}