using System.Threading.Tasks;
using Cars.Models.Dto;
using Cars.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cars.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger, IAdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        [HttpGet("recruitments")]
        public async Task<IActionResult> GetRecruitments([FromBody] AddRecruitmentDto addRecruitmentDto)
        {
            //var res =  await _adminService.AddRecruitment(addRecruitmentDto);
            return Ok("");
        }
    }
}