using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Cars.Models.DataModels;

namespace Cars.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ApplicationUser> SetProfilePictureAsync(string userId, string profilePicture);

        public Task<List<string>> GetUserRoles(string userId);

        public string GetUserId(ClaimsPrincipal user);
        
        public string GetUserName(ClaimsPrincipal user);
    }
}