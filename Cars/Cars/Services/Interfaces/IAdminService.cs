using System.Threading.Tasks;
using Cars.Models.View;

namespace Cars.Services.Interfaces;

public interface IAdminService
{
    Task<PaginatedList<UserView>> GetUsersInRole(string roles, string searchTerm, int pageSize, int pageIndex);

    public Task<bool> AddRoleToUser(string userId, string roleName);

    public Task<bool> DeleteRoleFromUser(string userId, string roleName);
}