namespace ApplicationCore.Interfaces;

public interface IBlobStorageRepository
{
    public Task<string?> SaveToBlob(string cvPartnerUserId, string employeeImageUri);
    Task DeleteBlob(string blobUrlToBeDeleted);
}