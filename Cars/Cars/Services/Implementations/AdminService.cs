using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Managers.Interfaces;
using Cars.Models.DataModels;
using Cars.Models.View;
using Cars.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Cars.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAppUserManager _appUserManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(UserManager<ApplicationUser> userManager, IAppUserManager appUserManager)
        {
            _userManager = userManager;
            _appUserManager = appUserManager;
        }

        public async Task<PaginatedList<UserView>> GetUsersInRole(string roleName, string searchTerm,
            int pageSize, int pageIndex)
        {
            _appUserManager.CheckIfRoleExists(roleName);
            var res = await _appUserManager.GetFilteredUsers(roleName, searchTerm);
            var dest = res.Adapt<List<UserView>>();
            var paginated = PaginatedList<UserView>.CreateAsync(dest, pageIndex, pageSize);

            return paginated;
        }

        public async Task<bool> AddRoleToUser(string userId, string roleName)
        {
            _appUserManager.CheckIfRoleExists(roleName);
            var user = await _appUserManager.FindUser(userId);
            var res = await _userManager.AddToRoleAsync(user, roleName);
            return res.Succeeded;
        }

        public async Task<bool> DeleteRoleFromUser(string userId, string roleName)
        {
            _appUserManager.CheckIfRoleExists(roleName);
            var user = await _appUserManager.FindUser(userId);
            var res = await _userManager.RemoveFromRoleAsync(user, roleName);
            return res.Succeeded;
        }
    }
}