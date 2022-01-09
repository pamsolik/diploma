using System.Net;
using System.Net.Http.Headers;
using Core.Exceptions;
using Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;

namespace Services.Implementations;

public class FileUploadService : IFileUploadService
{
    public async Task<FilePath> SaveFile(IFormFile file, string userId)
    {
        ValidateFile(file);
        
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
    
    private static void ValidateFile(IFormFile file)
    {
        var name = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;

        if (file.Length <= 0)
            throw new AppBaseException(HttpStatusCode.BadRequest, "File lenght is equal to 0");
        
        if (name is null)
            throw new AppBaseException(HttpStatusCode.BadRequest, "File name cannot be empty");
    }
}