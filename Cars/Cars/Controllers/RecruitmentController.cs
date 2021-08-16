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
    [Authorize]
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
        public async Task<IActionResult> AddRecruitment(AddRecruitmentDto addRecruitmentDto)
        {
            var res =  await _recruitmentService.AddRecruitment(addRecruitmentDto);
            return Ok(res);
        }
    }
}
