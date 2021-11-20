using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Managers.Interfaces;
using Cars.Models.DataModels;
using Cars.Models.Dto;
using Cars.Models.Enums;
using Cars.Models.Exceptions;
using Castle.Core.Internal;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static Cars.Services.Other.FilterUtilities;
using static Cars.Services.Other.FileService;

namespace Cars.Managers.Implementations
{
    public class RecruitmentManager : IRecruitmentManager
    {
        private readonly ApplicationDbContext _context;

        public RecruitmentManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<City> FindOrCreateCity(CityDto city)
        {
            var existingCity = await _context.Cities.FirstOrDefaultAsync(CompareCities(city));

            if (existingCity is not null) return existingCity;
            
            existingCity = city.Adapt<City>();
            _context.Cities.Add(existingCity);

            return existingCity;
        }

        public async Task<EntityEntry<Recruitment>> UpdateRecruitment(Recruitment recruitment)
        {
            var res = _context.Recruitments.Add(recruitment);
            await _context.SaveChangesAsync();
            return res;
        }

        public async Task<Recruitment> FindById(int id) => await _context.Recruitments.FindAsync(id);

        public async Task<Recruitment> CloseRecruitment(int recruitmentId, List<RecruitmentToClose> recruitmentsToClose)
        {
            var recruitment = await FindById(recruitmentId);
            
            if (recruitment is null) 
                throw new AppBaseException(HttpStatusCode.NotFound, "Recruitment not found");
            if (recruitment.Status == RecruitmentStatus.Closed)
                throw new AppBaseException(HttpStatusCode.Forbidden, "Recruitment has allready been closed");
            
            recruitment.Status = RecruitmentStatus.Closed;
            foreach (var recruitmentApplication in recruitmentsToClose)
            {
                var application =
                    recruitment.Applications.FirstOrDefault(a => a.Id == recruitmentApplication.ApplicationId);
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
            var res = _context.Recruitments.Update(recruitment);
            await _context.SaveChangesAsync();
        }

        public async Task<EntityEntry<RecruitmentApplication>> AddApplication(RecruitmentApplication application)
        {
            var res = _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            return res;
        }

        public async Task<List<RecruitmentApplication>> GetRecruitmentApplications(int recruitmentId) =>
            await _context.Applications
                .Where(a => a.RecruitmentId == recruitmentId)
                .ToListAsync();
        
        
        public IQueryable<Recruitment> GetRecruitments(RecruitmentMode recruitmentMode, string userId = "")
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

        public string MoveApplicationFileAndGetUrl(string parameter, int id, string subfolder)
        {
            if (parameter.IsNullOrEmpty()) return "";
            var path = Path.Combine("Resources", "Files", subfolder);
            return MoveAndGetUrl(parameter, id.ToString(), path, subfolder);
        }
    }
}