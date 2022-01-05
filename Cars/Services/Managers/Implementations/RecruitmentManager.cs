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

public class RecruitmentManager : IRecruitmentManager
{
    private readonly ApplicationDbContext _context;

    public RecruitmentManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<City?> FindOrCreateCity(CityDto? city)
    {
        var existingCity = await _context.Cities.FirstOrDefaultAsync(CompareCities(city));

        if (existingCity is not null) return existingCity;

        if (city != null) existingCity = city.Adapt<City>();
        _context.Cities.Add(existingCity ?? throw new InvalidOperationException("Mapping failure FindOrCreateCity"));

        return existingCity;
    }

    public async Task<EntityEntry<Recruitment>> UpdateRecruitment(Recruitment recruitment)
    {
        var res = _context.Recruitments.Update(recruitment);
        await _context.SaveChangesAsync();
        return res;
    }

    public async Task<EntityEntry<Recruitment>> SaveRecruitment(Recruitment recruitment)
    {
        var res = _context.Recruitments.Add(recruitment);
        await _context.SaveChangesAsync();
        return res;
    }

    public async Task<Recruitment> FindById(int id)
    {
        return await _context.Recruitments.FindAsync(id) ??
               throw new AppBaseException(HttpStatusCode.NotFound, "Recruitment not found");
    }

    public async Task<Recruitment?> CloseRecruitment(int recruitmentId, List<RecruitmentToClose>? recruitmentsToClose)
    {
        var recruitment = await FindById(recruitmentId);

        if (recruitment is null)
            throw new AppBaseException(HttpStatusCode.NotFound, "Recruitment not found");
        if (recruitment.Status == RecruitmentStatus.Closed)
            throw new AppBaseException(HttpStatusCode.Forbidden, "Recruitment has allready been closed");

        recruitment.Status = RecruitmentStatus.Closed;
        if (recruitmentsToClose != null)
            foreach (var recruitmentApplication in recruitmentsToClose)
            {
                var application =
                    recruitment.Applications?.FirstOrDefault(a => a.Id == recruitmentApplication.ApplicationId);
                if (application is not null) application.Selected = recruitmentApplication.Selected;
            }

        _context.Recruitments.Update(recruitment);
        await _context.SaveChangesAsync();
        return recruitment;
    }

    public async Task ChangeRecruitmentStatus(int id, RecruitmentStatus status)
    {
        var recruitment = await FindById(id);

        recruitment.Status = status;
        _ = _context.Recruitments.Update(recruitment);
        await _context.SaveChangesAsync();
    }

    public async Task<EntityEntry<RecruitmentApplication>> AddApplication(RecruitmentApplication application)
    {
        var res = _context.Applications.Add(application);
        await _context.SaveChangesAsync();
        return res;
    }

    public async Task<List<RecruitmentApplication>> GetRecruitmentApplications(int recruitmentId)
    {
        return await _context.Applications
            .Where(a => a.RecruitmentId == recruitmentId)
            .ToListAsync();
    }


    public IQueryable<Recruitment> GetRecruitments(RecruitmentMode recruitmentMode, string? userId = "")
    {
        return recruitmentMode switch
        {
            RecruitmentMode.Public => _context.Recruitments.Where(r => r.Status == RecruitmentStatus.Open),
            RecruitmentMode.Recruiter => _context.Recruitments.Where(r => r.RecruiterId == userId),
            RecruitmentMode.Admin => _context.Recruitments,
            _ => throw new ArgumentException("This mode doesn't exist")
        };
    }

    public async Task<EntityEntry<RecruitmentApplication>> CopyAndSaveApplicationFiles(
        AddApplicationDto applicationDto,
        EntityEntry<RecruitmentApplication> res)
    {
        if (applicationDto.ClFile.IsNullOrEmpty() && applicationDto.CvFile.IsNullOrEmpty()) return res;

        res.Entity.ClFile = MoveApplicationFileAndGetUrl(applicationDto.ClFile, res.Entity.Id, "CL");

        res.Entity.CvFile = MoveApplicationFileAndGetUrl(applicationDto.CvFile, res.Entity.Id, "CV");

        res = _context.Applications.Update(res.Entity);
        await _context.SaveChangesAsync();

        return res;
    }

    public string? MoveApplicationFileAndGetUrl(string? parameter, int id, string subfolder)
    {
        if (parameter.IsNullOrEmpty()) return "";
        var path = Path.Combine("Resources", "Files", subfolder);
        return MoveAndGetUrl(parameter, id.ToString(), path, subfolder);
    }
}