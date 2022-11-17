using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;

using Microsoft.Extensions.Options;

using Shared;

namespace BlobStorage.Repositories;

public class BlobStorageRepository: IBlobStorageRepository
{
    private readonly IOptionsSnapshot<AppSettings> _settings;

    /**
     * <summary> Class for handling blob storage </summary>
     * <param name="settings"> Options for connection string and which container to add to </param>
     */
    public BlobStorageRepository(IOptionsSnapshot<AppSettings> settings)
    {
        _settings = settings;
    }

    /**
     * <summary>Take a employee img url and convert it to a blob in a blob container. Converts the uri string to an actual URI and
     * copies the img from the url to the store</summary>
     * <param name="employeeName">Name of the employee. Will be the name of the file in the blob storage </param>
     * <param name="employeeImageUri"></param>
     * <param name="employeeImageUrl">URL to the employee image. Used to copy it to the blob storage</param>
     * 
     */
    public async Task<string> SaveToBlob(string employeeName, string employeeImageUri)
    {
        Uri uri = new(employeeImageUri);
        BlobContainerClient container = new(_settings.Value.BlobStorage.ConnectionString.ToString(), _settings.Value.BlobStorage.ContainerName);
        var blockBlobClient = container.GetBlockBlobClient(employeeName + ".png");
        var status = await blockBlobClient.StartCopyFromUriAsync(uri);
        await status.WaitForCompletionAsync();
        return blockBlobClient.Uri.AbsoluteUri;
    }
}