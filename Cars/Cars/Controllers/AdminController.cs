using System.Threading.Tasks;
using Cars.Models.Dto;
using Cars.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cars.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger, IAdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] string searchTerm, 
            [FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            var res = await _adminService.GetUsersInRole("User", searchTerm, pageSize, pageIndex);
            return Ok(res);
        }

        [HttpGet("recruiters")]
        public async Task<IActionResult> GetRecruiters([FromQuery] string searchTerm, 
            [FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            var res = await _adminService.GetUsersInRole("Recruiter", searchTerm, pageSize, pageIndex);
            return Ok(res);
        }

        [HttpGet("admins")]
        public async Task<IActionResult> GetAdmins([FromQuery] string searchTerm, 
            [FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            var res = await _adminService.GetUsersInRole("Admin", searchTerm, pageSize, pageIndex);
            return Ok(res);
        }

        [HttpPut("roles/add")]
        public async Task<IActionResult> AddRoleToUser([FromBody] EditRolesDto editRolesDto)
        {
            var res = await _adminService.AddRoleToUser(editRolesDto.UserId, editRolesDto.Role);
            return Ok(res);
        }

        [HttpPut("roles/remove")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] EditRolesDto editRolesDto)
        {
            var res = await _adminService.DeleteRoleFromUser(editRolesDto.UserId, editRolesDto.Role);
            return Ok(res);
        }
    }
}