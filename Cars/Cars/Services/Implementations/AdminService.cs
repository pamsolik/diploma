using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Models.Exceptions;
using Cars.Models.View;
using Cars.Services.Interfaces;
using Castle.Core.Internal;
using Mapster;
using Microsoft.AspNetCore.Identity;


namespace Cars.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(ApplicationDbContext context, IDateTimeProvider dateTimeProvider,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _userManager = userManager;
        }

        public async Task<PaginatedList<UserView>> GetUsersInRole(string roleName, string searchTerm, 
            int pageSize, int pageIndex)
        {
            CheckIfRoleExists(roleName);
            var res = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
            
            if (!searchTerm.IsNullOrEmpty())
                res = res.FindAll(r =>
                    r.Name.Contains(searchTerm) ||
                    r.Surname.Contains(searchTerm) ||
                    r.Email.Contains(searchTerm) ||
                    r.UserName.Contains(searchTerm));

            var dest = res.Adapt<List<UserView>>();
            var paginated = PaginatedList<UserView>.CreateAsync(dest, pageIndex, pageSize);

            return paginated; 
        }

        public async Task<bool> AddRoleToUser(string userId, string roleName)
        {
            CheckIfRoleExists(roleName);
            var user = await FindUser(userId);
            var res = await _userManager.AddToRoleAsync(user, roleName);
            return res.Succeeded;
        }

        public async Task<bool> DeleteRoleFromUser(string userId, string roleName)
        {
            CheckIfRoleExists(roleName);
            var user = await FindUser(userId);
            var res = await _userManager.RemoveFromRoleAsync(user, roleName);
            return res.Succeeded;
        }
        
        private void CheckIfRoleExists(string roleName)
        {
            if (!_context.Roles.Any(r => r.Name.Equals(roleName)))
                throw new AppBaseException(HttpStatusCode.NotFound, $"Role {roleName} not found");
        }
        
        private async Task<ApplicationUser> FindUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new AppBaseException(HttpStatusCode.NotFound, $"User {userId} not found");
            return user;
        }
    }
}