using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cars.Models.DataModels;
using Cars.Models.Dto;
using Cars.Models.Exceptions;
using Cars.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        public AdminController(ILogger<AdminController> logger, IAdminService adminService, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _logger = logger;
            _adminService = adminService;
            _userManager = userManager;
            _userService = userService;
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
            UserCannotEditHisRolesCheck(editRolesDto.UserId);
            var res = await _adminService.AddRoleToUser(editRolesDto.UserId, editRolesDto.Role);
            return Ok(res);
        }

        [HttpPut("roles/remove")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] EditRolesDto editRolesDto)
        {
            UserCannotEditHisRolesCheck(editRolesDto.UserId);
            var res = await _adminService.DeleteRoleFromUser(editRolesDto.UserId, editRolesDto.Role);
            return Ok(res);
        }
        
        [HttpGet("roles/{userId}")]
        public async Task<IActionResult> GetClientRoles(string userId)
        {
            var uId = await _userManager.FindByIdAsync(userId);
            if (uId is null) return NotFound($"User with ID {userId} not found.");
            var res = await _userService.GetUserRoles(userId);
            return Ok(res);
        }
        
        private void UserCannotEditHisRolesCheck(string userId)
        {
            if (_userService.GetUserId(User) == userId)
                throw new AppBaseException(HttpStatusCode.Forbidden, "Nie można edytować własnych ról");
        }
    }
}