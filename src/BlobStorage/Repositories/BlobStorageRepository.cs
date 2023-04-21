using System.Globalization;

using Azure.Storage.Blobs;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Shared;

namespace BlobStorage.Repositories;

public class BlobStorageRepository : IBlobStorageRepository
{
    private readonly IOptionsSnapshot<AppSettings> _appSettings;
    private ILogger<BlobStorageRepository> _logger;
    
    private const string BlobMetadataLastModifiedKey = "lastModified";

    /**
     * <summary> Class for handling blob storage </summary>
     * <param name="appSettings"> Options for connection string and which container to add to </param>
     */
    public BlobStorageRepository(IOptionsSnapshot<AppSettings> appSettings, ILogger<BlobStorageRepository> logger)
    {
        _appSettings = appSettings;
        _logger = logger;
    }

    /**
     * <summary>Copies the image at employeeImageUri and streams it into a Blob Block in Azure</summary>
     * <param name="cvPartnerUserId">Name of the employee. Will be the name of the file in the blob storage</param>
     * <param name="employeeImageUri">URL to the employee image. Used to copy it to the blob storage</param>
     * <param name="updatedAt">Last time the CV was updated. A naive way of uploading pictures to blob less often</param>
     */
    public async Task<string> SaveToBlob(string cvPartnerUserId, string employeeImageUri)
    {
        Uri uri = new(employeeImageUri);

        BlobContainerClient container = _appSettings.Value.BlobStorage.UseDevelopmentStorage
            ? new BlobContainerClient("UseDevelopmentStorage=true", "employees")
            : new BlobContainerClient(_appSettings.Value.BlobStorage.Endpoint);
        
        await container.CreateIfNotExistsAsync();

        var blockBlobClient = container.GetBlobClient($"{cvPartnerUserId}.png");

        using var client = new HttpClient();
        var message = new HttpRequestMessage(HttpMethod.Options, uri);
        var response = await client.SendAsync(message);
        var lastModified = response.Content.Headers.LastModified.ToString() ?? DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture);

        if (await blockBlobClient.ExistsAsync())
        {
            var props = await blockBlobClient.GetPropertiesAsync();
            if (props.HasValue)
            {
                props.Value.Metadata.TryGetValue(BlobMetadataLastModifiedKey, out string? blobLastModified);

                if (blobLastModified != null && blobLastModified == lastModified)
                {
                    // No need to update the blob
                    _logger.LogInformation(
                        "No need to update the blob for {CvPartnerUserId} as the image has not changed",
                        cvPartnerUserId);
                    return blockBlobClient.Uri.AbsoluteUri;
                }
            }
        }

        var stream = await blockBlobClient.OpenWriteAsync(true);
        await stream.WriteAsync(await new HttpClient().GetByteArrayAsync(uri));
        await stream.FlushAsync();
        await stream.DisposeAsync();

        await blockBlobClient.SetMetadataAsync(new Dictionary<string, string>()
        {
            { "name", cvPartnerUserId }, { BlobMetadataLastModifiedKey, lastModified }
        });

        return blockBlobClient.Uri.AbsoluteUri;
    }
}