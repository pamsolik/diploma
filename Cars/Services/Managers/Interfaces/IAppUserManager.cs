using System.Security.Claims;
using Cars.Models;
using Core.DataModels;

namespace Services.Managers.Interfaces;

public interface IAppUserManager
{
    public Task<ApplicationUser> SetProfilePictureAsync(string userId, string profilePicture);

    public Task<List<string>> GetUserRoles(string userId);

    public string GetUserId(ClaimsPrincipal user);

    public string GetUserName(ClaimsPrincipal user);

    public Task<ApplicationUser> FindUser(string userId);

    public Task<List<ApplicationUser>> GetFilteredUsers(string roleName, string searchTerm);

    public void CheckIfRoleExists(string roleName);
}