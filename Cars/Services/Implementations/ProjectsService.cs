using System.Globalization;
using Core.DataModels;
using Core.Dto;
using Core.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Services.Interfaces;
using Services.Managers.Implementations;
using Services.Extensions;
using Services.Managers.Interfaces;

namespace Services.Implementations;

public class ProjectsService : IProjectsService
{
    private readonly IProjectsManager _projectManager;
    public ProjectsService(IProjectsManager projectManager)
    {
        _projectManager = projectManager;
    }

    public async Task AddProjects(List<ProjectDto> projects)
    {
        var dest = projects.Adapt<List<Project>>();
        await _projectManager.AddProjectsAsync(dest);
    }

    public async Task<PaginatedList<ProjectView>> GetProjectsFiltered(ProjectsFilterDto filter)
    {
        var projects = await _projectManager.GetProjects(filter);
        var dest = projects.Adapt<List<ProjectView>>();
        var paginated = PaginatedList<ProjectView>.CreateAsync(dest, filter.PageIndex, filter.PageSize);

        return paginated;
    }

    public async Task<string> GetProjectsAsCsv(ProjectsFilterDto filter)
    {
        var projects = await _projectManager.GetProjects(filter);
        var dest = projects.Adapt<List<ProjectView>>();
        return dest.ToCsv<ProjectView>();
    }

}