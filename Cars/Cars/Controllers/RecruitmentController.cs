using Cars.Models.Dto;
using Cars.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cars.Controllers
{
    //[Authorize]
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
            try
            {
                var res =  await _recruitmentService.AddRecruitment(addRecruitmentDto);
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        
        [HttpPut]
        public async Task<IActionResult> EditRecruitment([FromBody] EditRecruitmentDto editRecruitment)
        {
            try
            {
                var res =  await _recruitmentService.EditRecruitment(editRecruitment);
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetRecruitments()
        {
            try
            {
                var res =  await _recruitmentService.GetRecruitments();
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        
        [HttpPost("filtered")]
        public async Task<IActionResult> GetRecruitmentsFiltered([FromBody] RecruitmentFilterDto recruitmentFilterDto)
        {
            try
            {
                var res = await _recruitmentService.GetRecruitmentsFiltered(recruitmentFilterDto);
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
