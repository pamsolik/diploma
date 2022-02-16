using Core.Dto;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Implementations;

namespace Cars.Controllers;

[Authorize(Roles = "User,Recruiter,Admin")]
[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly ILogger<ProjectsController> _logger;
    private readonly IProjectsService _projectsService;

    public ProjectsController(ILogger<ProjectsController> logger, IProjectsService projectsService)
    {
        _logger = logger;
        _projectsService = projectsService;
    }

    [Authorize(Roles = "Recruiter,Admin")]
    [HttpPost]
    public async Task<IActionResult> GetProjectsFiltered([FromBody] ProjectsFilterDto filter)
    {
        var res = await _projectsService.GetProjectsFiltered(filter);
        return Ok(res);
    }  

    [Authorize(Roles = "Recruiter,Admin")]
    [HttpPost("csv")]
    public async Task<IActionResult> GetProjectsAsCsv([FromBody] ProjectsFilterDto filter)
    {
        var res = await _projectsService.GetProjectsAsCsv(filter);
        return Ok(new ApiAnswer(res));
    }  

    [Authorize(Roles = "Recruiter,Admin")]
    [HttpPut]
    public async Task<IActionResult> AddProjectsToScan([FromBody] List<ProjectDto> projects)
    {
        await _projectsService.AddProjects(projects);
        return Ok(new ApiAnswer("Projects added"));
    }   
}