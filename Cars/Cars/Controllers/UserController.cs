using Core.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Managers.Interfaces;

namespace Cars.Controllers;

[Authorize]
[ApiController]
[Route("api/user/auth")]
public class UserController : ControllerBase
{
    private readonly IAppUserManager _appUserManager;

    public UserController(IAppUserManager appUserManager)
    {
        _appUserManager = appUserManager;
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetUserRoles()
    {
        var uId = _appUserManager.GetUserId(User);
        if (uId is null) return Ok(new List<string>());
        var res = await _appUserManager.GetUserRoles(uId);
        return Ok(res);
    }

    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserData()
    {
        var uId = _appUserManager.GetUserId(User);
        var user = await _appUserManager.FindUser(uId);
        var res = user.Adapt<UserView>();
        return Ok(res);
    }
}