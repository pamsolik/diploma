using Core.Dto;
using Core.ViewModels;

namespace Services.Implementations;

public interface IProjectsService
{
    Task<PaginatedList<ProjectView>> GetProjectsFiltered(ProjectsFilterDto filter);

    Task<string> GetProjectsAsCsv(ProjectsFilterDto filter);
    Task AddProjects(List<ProjectDto> projects);
}