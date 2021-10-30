using System.Net;
using System.Threading.Tasks;
using Cars.Models.Dto;
using Cars.Models.Enums;
using Cars.Models.Exceptions;
using Cars.Models.View;
using Cars.Services.Interfaces;
using Cars.Services.Other;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Namotion.Reflection;

namespace Cars.Controllers
{
    //[Authorize(Roles = "Recruiter,Admin")]
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

        [HttpPost]
        public async Task<IActionResult> AddRecruitment([FromBody] AddRecruitmentDto addRecruitmentDto)
        {
            var usr = User.Identity.GetUserId();
            var res = await _recruitmentService.AddRecruitment(addRecruitmentDto, usr);
            
            return Ok(new ApiAnswer("Added", res));
        }

        [HttpPut]
        public async Task<IActionResult> EditRecruitment([FromBody] EditRecruitmentDto editRecruitment)
        {
            if (User.Identity.GetUserId() != editRecruitment.RecruiterId)
                throw new AppBaseException(HttpStatusCode.Forbidden,
                    "User is not authorised to edit this recruitment.");
            var res = await _recruitmentService.EditRecruitment(editRecruitment);
            return Ok(new ApiAnswer("Edited", res));
        }

        [HttpPut("status/{id:int}")]
        public IActionResult CloseRecruitment([FromBody] RecruitmentStatus status, [FromRoute] int id)
        {
            //TODO: Maybe delete
            //var res = await _recruitmentService.();
            return Ok("Not implemented " + status);
        }

        [HttpPost("public")]
        public async Task<IActionResult> GetRecruitmentsPublic([FromBody] RecruitmentFilterDto recruitmentFilterDto)
        {
            var res = await _recruitmentService.GetRecruitmentsFiltered(recruitmentFilterDto, RecruitmentMode.Public);
            return Ok(res);
        }

        [HttpPost("recruiter")]
        public async Task<IActionResult> GetRecruitmentsRecruiter([FromBody] RecruitmentFilterDto recruitmentFilterDto)
        {
            var res = await _recruitmentService.GetRecruitmentsFiltered(recruitmentFilterDto, RecruitmentMode.Recruiter,
                User.Identity.GetUserId());
            return Ok(res);
        }

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