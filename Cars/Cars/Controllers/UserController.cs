using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Models.Dto;
using Cars.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cars.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        
        [HttpGet("auth/roles")]
        public async Task<IActionResult> GetClientRoles()
        {
            var uId = _userService.GetUserId(User);
            if (uId is null) return Ok(new List<string>());
            var res = await _userService.GetUserRoles(uId);
            return Ok(res);
        }
    }
}