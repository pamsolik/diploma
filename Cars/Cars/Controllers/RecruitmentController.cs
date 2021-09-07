using Cars.Models.Dto;
using Cars.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cars.Models.Exceptions;
using IdentityServer4.EntityFramework.Entities;

namespace Cars.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("recruitment")]
    public class RecruitmentController : ControllerBase
    {
        private readonly IRecruitmentService _recruitmentService;
        private readonly ILogger<RecruitmentController> _logger;

        public RecruitmentController(ILogger<RecruitmentController> logger, IRecruitmentService recruitmentService)
        {
            _recruitmentService = recruitmentService;
            _logger = logger;
        }
        
        [HttpPost]
        public async Task<IActionResult> AddRecruitment([FromBody] AddRecruitmentDto addRecruitmentDto)
        {
            var res =  await _recruitmentService.AddRecruitment(addRecruitmentDto);
            return Ok(res);
        }
        
        [HttpPut]
        public async Task<IActionResult> EditRecruitment([FromBody] EditRecruitmentDto editRecruitment)
        {
            var res =  await _recruitmentService.EditRecruitment(editRecruitment);
            return Ok(res);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetRecruitments()
        {
            var res =  await _recruitmentService.GetRecruitments();
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
        
        [HttpGet("applications/:id")]
        public async Task<IActionResult> GetApplications([FromRoute] int id)
        {
            var res = await _recruitmentService.GetApplications(id);
            return Ok(res);
        }
    }
}
