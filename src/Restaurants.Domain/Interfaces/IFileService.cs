using Microsoft.AspNetCore.Http;

namespace Restaurants.Domain.Interfaces;

public interface IFileService
{
    bool IsFileValid(IFormFile file);
    bool IsFileValid(List<IFormFile> files);
    Stream GetFileStream(IFormFile file);
    Task<string?> UploadFileAsync(IFormFile? file);
}
