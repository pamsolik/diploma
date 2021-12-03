using System.Net;
using System.Threading.Tasks;
using Cars.Managers.Interfaces;
using Cars.Models.Dto;
using Cars.Models.Enums;
using Cars.Models.Exceptions;
using Cars.Models.View;
using Cars.Services.Interfaces;
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
        private readonly IAppUserManager _appUserManager;
        private readonly ILogger<RecruitmentController> _logger;

        private readonly IRecruitmentService _recruitmentService;

        public RecruitmentController(ILogger<RecruitmentController> logger, IRecruitmentService recruitmentService,
            IAppUserManager appUserManager)
        {
            _recruitmentService = recruitmentService;
            _appUserManager = appUserManager;
            _logger = logger;
        }

        [Authorize(Roles = "Recruiter,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddRecruitment([FromBody] AddRecruitmentDto addRecruitmentDto)
        {
            var usr = _appUserManager.GetUserId(User);
            var res = await _recruitmentService.AddRecruitment(addRecruitmentDto, usr);

            return Ok(new ApiAnswer("Added", res));
        }

        [Authorize(Roles = "Recruiter,Admin")]
        [HttpPut]
        public async Task<IActionResult> EditRecruitment([FromBody] EditRecruitmentDto editRecruitment)
        {
            if (_appUserManager.GetUserId(User) != editRecruitment.RecruiterId)
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
            var res = await _recruitmentService.GetRecruitmentsFiltered(recruitmentFilterDto, RecruitmentMode.Public);
            return Ok(res);
        }

        [Authorize(Roles = "Recruiter,Admin")]
        [HttpPost("recruiter")]
        public async Task<IActionResult> GetRecruitmentsRecruiter([FromBody] RecruitmentFilterDto recruitmentFilterDto)
        {
            var res = await _recruitmentService.GetRecruitmentsFiltered(recruitmentFilterDto, RecruitmentMode.Recruiter,
                _appUserManager.GetUserId(User));
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
            var applicantId = _appUserManager.GetUserId(User);
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