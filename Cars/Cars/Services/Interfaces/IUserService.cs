using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Models.DataModels;
using Cars.Models.View;

namespace Cars.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ApplicationUser> SetProfilePictureAsync(string userId, string profilePicture);
    }
}