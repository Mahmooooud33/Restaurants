using Microsoft.AspNetCore.Http;

namespace Restaurants.Domain.Interfaces;

public interface IFileService
{
    bool IsFileValid(IFormFile file);
    bool IsFilesValid(List<IFormFile> files);
    Task<string?> UploadFileAsync(IFormFile? file);
}
