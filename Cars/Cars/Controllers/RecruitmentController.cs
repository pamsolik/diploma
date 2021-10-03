using System;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Cars.Models.Dto;
using Cars.Models.Enums;
using Cars.Models.Exceptions;
using Cars.Models.View;
using Cars.Services.Interfaces;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;

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
           
            //TODO: TEST
            return Ok(new ApiAnswer("Added"));
        }

        [HttpPut]
        public async Task<IActionResult> EditRecruitment([FromBody] EditRecruitmentDto editRecruitment)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != editRecruitment.RecruiterId)
                throw new AppBaseException(HttpStatusCode.Forbidden,
                    "User is not authorised to edit this recruitment.");
            var res = await _recruitmentService.EditRecruitment(editRecruitment);
            return Ok(new ApiAnswer("Edited"));
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> CloseRecruitment([FromBody] RecruitmentStatus status)
        {
            //TODO: Maybe delete
            var res = await _recruitmentService.GetRecruitments(User.GetSubjectId());
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetRecruitments()
        {
            var res = await _recruitmentService.GetRecruitments(User.GetSubjectId());
            return Ok(res);
        }

        [HttpPost("filtered")]
        public async Task<IActionResult> GetRecruitmentsFiltered([FromBody] RecruitmentFilterDto recruitmentFilterDto)
        {
            var res = await _recruitmentService.GetRecruitmentsFiltered(recruitmentFilterDto);
            return Ok(res);
        }

        [HttpPost("apply")]
        public async Task<IActionResult> Apply([FromBody] AddApplicationDto addApplicationDto)
        {
            var res = await _recruitmentService.AddApplication(addApplicationDto);
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