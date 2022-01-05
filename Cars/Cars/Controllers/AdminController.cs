using System.Net;
using Core.DataModels;
using Core.Dto;
using Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Managers.Interfaces;

namespace Cars.Controllers;

//[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IAppUserManager _appUserManager;
    private readonly ILogger<AdminController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(ILogger<AdminController> logger, IAdminService adminService,
        UserManager<ApplicationUser> userManager, IAppUserManager appUserManager)
    {
        _logger = logger;
        _adminService = adminService;
        _userManager = userManager;
        _appUserManager = appUserManager;
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
        _logger.LogInformation("Role {Role} added to user: {UserId}", editRolesDto.Role, editRolesDto.UserId);
        return Ok(res);
    }

    [HttpPut("roles/remove")]
    public async Task<IActionResult> RemoveRoleFromUser([FromBody] EditRolesDto editRolesDto)
    {
        UserCannotEditHisRolesCheck(editRolesDto.UserId);
        var res = await _adminService.DeleteRoleFromUser(editRolesDto.UserId, editRolesDto.Role);
        _logger.LogInformation("Role {Role} removed from user: {UserId}", editRolesDto.Role, editRolesDto.UserId);
        return Ok(res);
    }

    [HttpGet("roles/{userId}")]
    public async Task<IActionResult> GetClientRoles(string userId)
    {
        var uId = await _userManager.FindByIdAsync(userId);
        if (uId is null) return NotFound($"User with ID {userId} not found.");
        var res = await _appUserManager.GetUserRoles(userId);
        return Ok(res);
    }

    private void UserCannotEditHisRolesCheck(string userId)
    {
        if (_appUserManager.GetUserId(User) == userId)
            throw new AppBaseException(HttpStatusCode.Forbidden, "User cannot edit his own roles.");
    }
}