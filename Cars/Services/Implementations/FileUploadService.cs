using System.Net.Http.Headers;
using Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;

namespace Services.Implementations;

public class FileUploadService : IFileUploadService
{
    public async Task<FilePath> SaveFile(IFormFile file, string userId)
    {
        var folderName = Path.Combine("Resources", "Temp");
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        var name = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;

        var fileName = $"File_{userId}_{DateTime.Now:yyyy-dd-MM-HH-mm-ss}{Path.GetExtension(name)?.Trim('"')}";
        var fullPath = Path.Combine(pathToSave, fileName);
        var dbPath = Path.Combine(folderName, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
        return new FilePath(dbPath);
    }
}