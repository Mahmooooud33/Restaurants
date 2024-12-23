﻿using Microsoft.AspNetCore.Http;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Storage;

internal class FileService(IBlobStorageService blobStorageService) : IFileService
{
    private readonly List<string> _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxAllowedFileSize = 1048576; // 1MB

    public bool IsFileValid(IFormFile file)
    {
        if(file is null) 
            return false;

        string extension = Path.GetExtension(file.FileName);
        return file.Length > 0 && file.Length <= MaxAllowedFileSize 
                               && _allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase) 
                               && file.ContentType.Contains("image");
    }

    public bool IsFilesValid(List<IFormFile> files) 
        => files.Count > 0 && files.All(IsFileValid);

    public async Task<string?> UploadFileAsync(IFormFile? file)
    {
        if(!IsFileValid(file!))
            return null;

        using (var stream = file!.OpenReadStream())
        {
            var blobUri = await blobStorageService.UploadToBlobAsync(stream, file.FileName);
            return blobUri;
        }
    }
}