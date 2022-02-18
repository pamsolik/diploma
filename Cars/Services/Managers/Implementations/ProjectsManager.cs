using Core.DataModels;
using Core.Dto;
using Duende.IdentityServer.Extensions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Services.Managers.Implementations;

public class ProjectsManager : IProjectsManager
{
    private readonly ApplicationDbContext _context;

    public ProjectsManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddProjectsAsync(List<Project> projects)
    {
        await _context.Projects.AddRangeAsync(projects);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Project>> GetProjects(ProjectsFilterDto filter)
    {
        var res = _context.Projects.Select(p => p);
        if (!filter.SearchString.IsNullOrEmpty()) 
        {
            var search = filter.SearchString.ToLower();
            res = res.Where(p => p.Title.ToLower().Contains(search) 
            || p.Description.ToLower().Contains(search));
        }

        if (filter.Technology is not null)
            res = res.Where(p => p.Technology == filter.Technology);

        if (filter.DateFrom is not null)
            res = res.Where(p => p.CodeQualityAssessment != null && p.CodeQualityAssessment.CompletedTime >= filter.DateFrom);

        if (filter.DateTo is not null)
            res = res.Where(p => p.CodeQualityAssessment != null && p.CodeQualityAssessment.CompletedTime <= filter.DateTo);


        return await res.ToListAsync();
    }
}