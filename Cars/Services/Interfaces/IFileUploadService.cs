using Core.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Services.Interfaces;

public interface IFileUploadService
{
    public Task<FilePath> SaveFile(IFormFile file, string userId);
}