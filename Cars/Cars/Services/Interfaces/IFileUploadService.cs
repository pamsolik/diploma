using System.Threading.Tasks;
using Cars.Models.View;
using Microsoft.AspNetCore.Http;

namespace Cars.Services.Interfaces
{
    public interface IFileUploadService
    {
        public Task<FilePath> SaveFile(IFormFile file, string userId);
    }
}