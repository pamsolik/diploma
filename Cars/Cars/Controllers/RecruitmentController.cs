using System.Net;
using System.Threading.Tasks;
using Cars.Models.Dto;
using Cars.Models.Enums;
using Cars.Models.Exceptions;
using Cars.Models.View;
using Cars.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cars.Controllers
{
    [Authorize(Roles = "User,Recruiter,Admin")]
    [ApiController]
    [Route("api/recruitments")]
    public class RecruitmentController : ControllerBase
    {
        private readonly ILogger<RecruitmentController> _logger;

        private readonly IRecruitmentService _recruitmentService;

        public RecruitmentController(ILogger<RecruitmentController> logger, IRecruitmentService recruitmentService)
        {
            _recruitmentService = recruitmentService;
            _logger = logger;
        }
        
        [Authorize(Roles = "Recruiter,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddRecruitment([FromBody] AddRecruitmentDto addRecruitmentDto)
        {
            var usr = User.Identity.GetUserId();
            var res = await _recruitmentService.AddRecruitment(addRecruitmentDto, usr);

            return Ok(new ApiAnswer("Added", res));
        }

        [Authorize(Roles = "Recruiter,Admin")]
        [HttpPut]
        public async Task<IActionResult> EditRecruitment([FromBody] EditRecruitmentDto editRecruitment)
        {
            if (User.Identity.GetUserId() != editRecruitment.RecruiterId)
                throw new AppBaseException(HttpStatusCode.Forbidden,
                    "User is not authorised to edit this recruitment.");
            var res = await _recruitmentService.EditRecruitment(editRecruitment);
            return Ok(new ApiAnswer("Edited", res));
        }

        [Authorize(Roles = "Recruiter,Admin")]
        [HttpPut("close")]
        public async Task<IActionResult> CloseRecruitment([FromBody] CloseRecruitmentDto closeRecruitmentDto)
        {
            await _recruitmentService.CloseRecruitment(closeRecruitmentDto);
            return Ok(new ApiAnswer("Closed"));
        }

        [Authorize(Roles = "Recruiter,Admin")]
        [HttpPut("hide/{id:int}")]
        public async Task<IActionResult> HideRecruitment([FromRoute] int id)
        {
            await _recruitmentService.HideRecruitment(id);
            return Ok(new ApiAnswer("Hidden"));
        }

        [Authorize(Roles = "Recruiter,Admin")]
        [HttpPut("unhide/{id:int}")]
        public async Task<IActionResult> UnHideRecruitment([FromRoute] int id)
        {
            await _recruitmentService.UnHideRecruitment(id);
            return Ok(new ApiAnswer("UnHidden"));
        }

        
        [HttpPost("public")]
        public async Task<IActionResult> GetRecruitmentsPublic([FromBody] RecruitmentFilterDto recruitmentFilterDto)
        {
            if (!User.IsInRole("User")) throw new AppBaseException(HttpStatusCode.Forbidden, "Not in role");
            var res = await _recruitmentService.GetRecruitmentsFiltered(recruitmentFilterDto, RecruitmentMode.Public);
            return Ok(res);
        }

        [Authorize(Roles = "Recruiter,Admin")]
        [HttpPost("recruiter")]
        public async Task<IActionResult> GetRecruitmentsRecruiter([FromBody] RecruitmentFilterDto recruitmentFilterDto)
        {
            var res = await _recruitmentService.GetRecruitmentsFiltered(recruitmentFilterDto, RecruitmentMode.Recruiter,
                User.Identity.GetUserId());
            return Ok(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> GetRecruitmentsAdmin([FromBody] RecruitmentFilterDto recruitmentFilterDto)
        {
            var res = await _recruitmentService.GetRecruitmentsFiltered(recruitmentFilterDto, RecruitmentMode.Admin);
            return Ok(res);
        }
        
        [HttpPost("apply")]
        public async Task<IActionResult> Apply([FromBody] AddApplicationDto addApplicationDto)
        {
            var applicantId = User.Identity.GetUserId();
            var res = await _recruitmentService.AddApplication(addApplicationDto, applicantId);
            return Ok(res);
        }

        [Authorize(Roles = "Recruiter,Admin")]
        [HttpGet("applications/{id:int}")]
        public async Task<IActionResult> GetApplications([FromRoute] int id)
        {
            var res = await _recruitmentService.GetApplications(id);
            return Ok(res);
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRecruitmentDetails([FromRoute] int id)
        {
            var res = await _recruitmentService.GetRecruitmentDetails(id);
            return Ok(res);
        }
    }
}