using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Managers.Interfaces;
using Cars.Models.Dto;
using Cars.Models.View;
using Cars.Services.Interfaces;
using Mapster;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cars.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user/auth")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IAppUserManager _appUserManager;

        public UserController(ILogger<UserController> logger, IAppUserManager appUserManager)
        {
            _logger = logger;
            _appUserManager = appUserManager;
        }
        
        [HttpGet("roles")]
        public async Task<IActionResult> GetClientRoles()
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
}