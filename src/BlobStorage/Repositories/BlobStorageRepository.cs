using System.Globalization;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Shared;

namespace BlobStorage.Repositories;

public class BlobStorageRepository : IBlobStorageRepository
{
    private readonly IOptionsSnapshot<AppSettings> _appSettings;
    private ILogger<BlobStorageRepository> _logger;

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
    public async Task<string> SaveToBlob(string cvPartnerUserId, string employeeImageUri, DateTime updatedAt)
    {
        var updatedAtString = updatedAt.ToString(CultureInfo.InvariantCulture);
        Uri uri = new(employeeImageUri);

        BlobContainerClient container = new(_appSettings.Value.BlobStorage.ConnectionString.ToString(),
            _appSettings.Value.BlobStorage.ContainerName);
        await container.CreateIfNotExistsAsync();
        var blockBlobClient = container.GetBlobClient($"{cvPartnerUserId}.png");

        if (await blockBlobClient.ExistsAsync())
        {
            var props = await blockBlobClient.GetPropertiesAsync();
            if (props.HasValue)
            {
                props.Value.Metadata.TryGetValue("UpdatedAt", out string? oldUpdatedAt);
                if (oldUpdatedAt != null && oldUpdatedAt == updatedAtString)
                {
                    // No need to update the blob
                    _logger.LogInformation(
                        "No need to update the blob with {CvPartnerUserId} as {OldUpdatedAt} matched {UpdatedAtString}",
                        cvPartnerUserId, oldUpdatedAt, updatedAtString);
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
            { "Name", cvPartnerUserId }, { "UpdatedAt", updatedAtString }
        });

        return blockBlobClient.Uri.AbsoluteUri;
    }
}