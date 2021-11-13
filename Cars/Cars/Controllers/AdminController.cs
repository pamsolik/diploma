using System.Threading.Tasks;
using Cars.Services.Interfaces;
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

        // [HttpGet("recruitments")]
        // public async Task<IActionResult> GetRecruitments()
        // {
        //     //var res = await _adminService.GetRecruitments();
        //
        //     return Ok("");
        // }
    }
}