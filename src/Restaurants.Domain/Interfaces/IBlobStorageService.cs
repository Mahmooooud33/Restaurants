namespace Restaurants.Domain.Interfaces;

public interface IBlobStorageService
{
    string? GetBlobSasUrl(string? blobUrl);
    Task<string> UploadToBlobAsync(Stream stream, string fileName);
}