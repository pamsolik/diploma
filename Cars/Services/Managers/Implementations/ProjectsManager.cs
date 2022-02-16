using System.Net;
using Core.DataModels;
using Core.Dto;
using Core.Enums;
using Core.Exceptions;
using Duende.IdentityServer.Extensions;
using Infrastructure;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Managers.Interfaces;
using static Services.Other.FilterUtilities;
using static Services.Other.FileService;

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

    // public IQueryable<Project> GetProjects(RecruitmentMode recruitmentMode, string? userId = "")
    // {
    //     return recruitmentMode switch
    //     {
    //         RecruitmentMode.Public => _context.Recruitments.Where(r => r.Status == RecruitmentStatus.Open),
    //         RecruitmentMode.Recruiter => _context.Recruitments.Where(r => r.RecruiterId == userId),
    //         RecruitmentMode.Admin => _context.Recruitments,
    //         _ => throw new ArgumentException("This mode doesn't exist")
    //     };
    // }

    public async Task<List<Project>> GetProjects(ProjectsFilterDto filter)
    {
        return await _context.Projects.ToListAsync();
    }
}