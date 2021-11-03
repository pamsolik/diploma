using System.Threading.Tasks;
using Cars.Models.DataModels;

namespace Cars.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ApplicationUser> SetProfilePictureAsync(string userId, string profilePicture);
    }
}