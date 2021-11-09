using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Models.Dto;
using Cars.Models.Enums;
using Cars.Models.Exceptions;
using Cars.Models.View;
using Cars.Services.Interfaces;
using IdentityServer4.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using RestSharp;
using static Cars.Services.Other.FilterUtilities;
using static Cars.Services.Other.FileService;

namespace Cars.Services.Implementations
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly ApplicationDbContext _context;

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<RecruitmentService> _logger;

        public RecruitmentService(ApplicationDbContext context, ILogger<RecruitmentService> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> AddRecruitment(AddRecruitmentDto addRecruitmentDto, string recruiterId)
        {
            //TODO: check if valid and respond accordingly
            var existingCity = _context.Cities.FirstOrDefault(CompareCities(addRecruitmentDto));

            if (existingCity is null)
            {
                existingCity = addRecruitmentDto.City.Adapt<City>();
                _context.Cities.Add(existingCity);
            }

            var dest = addRecruitmentDto.Adapt<Recruitment>();

            dest.City = existingCity;
            dest.RecruiterId = recruiterId;
            dest.StartDate = _dateTimeProvider.GetTimeNow();
            var res = _context.Recruitments.Add(dest);
            await _context.SaveChangesAsync();

            res = await CopyAndSaveThumbnail(addRecruitmentDto, res);
            return res.Entity.Id;
        }

        public async Task<int> EditRecruitment(EditRecruitmentDto addRecruitmentDto)
        {
            var recruitment = await _context.Recruitments.FindAsync(addRecruitmentDto.Id);
            var dest = addRecruitmentDto.Adapt<Recruitment>(); //TODO: check if valid and respond accordingly
            var res = _context.Recruitments.Update(dest);
            await _context.SaveChangesAsync();
            return res.Entity.Id;
        }

        public async Task<bool> CloseRecruitment(CloseRecruitmentDto closeRecruitmentDto)
        {
            var recruitment = await _context.Recruitments.FindAsync(closeRecruitmentDto.RecruitmentId);
            
            if (recruitment is null) 
                throw new AppBaseException(HttpStatusCode.NotFound, "Recruitment not found");
            if (recruitment.Status == RecruitmentStatus.Closed)
                throw new AppBaseException(HttpStatusCode.Forbidden, "Recruitment has allready been closed");
            
            recruitment.Status = RecruitmentStatus.Closed;
            foreach (var recruitmentApplication in closeRecruitmentDto.RecruitmentsToClose)
            {
                var application =
                    recruitment.Applications.FirstOrDefault(a => a.Id == recruitmentApplication.ApplicationId);
                if (application is not null) application.Selected = recruitmentApplication.Selected;
            }

            _context.Recruitments.Update(recruitment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HideRecruitment(int id)
        {
            await ChangeRecruitmentStatus(id, RecruitmentStatus.Hidden);
            return true;
        }

        public async Task<bool> UnHideRecruitment(int id)
        {
            await ChangeRecruitmentStatus(id, RecruitmentStatus.Open);
            return true;
        }

        public async Task<RecruitmentDetailsView> GetRecruitmentDetails(int recruitmentId)
        {
            var res = await _context.Recruitments.FirstOrDefaultAsync(r => r.Id == recruitmentId);
            var dest = res.Adapt<RecruitmentDetailsView>();
            return dest;
        }

        public async Task<PaginatedList<RecruitmentView>>
            GetRecruitmentsFiltered(RecruitmentFilterDto filter, RecruitmentMode recruitmentMode, string userId = "")
        {
            var recruitments = GetRecruitments(recruitmentMode, userId);

            var recruitmentList = FilterOutAndSortRecruitments(ref recruitments, filter).ToList();
            var dest = recruitmentList.Adapt<List<RecruitmentView>>();
            GetDaysAgoDescriptions(ref dest);
            var paginated = PaginatedList<RecruitmentView>.CreateAsync(dest, filter.PageIndex, filter.PageSize);

            return paginated;
        }

        public async Task<RecruitmentApplication> AddApplication(AddApplicationDto addApplicationDto,
            string applicantId)
        {
            var dest = addApplicationDto.Adapt<RecruitmentApplication>(); //TODO: check if valid and respond accordingly
            dest.ApplicantId = applicantId;
            var res = _context.Applications.Add(dest);
            await _context.SaveChangesAsync();
            res = await CopyAndSaveApplicationFiles(addApplicationDto, res);
            return res.Entity;
        }

        public async Task<List<ApplicationView>> GetApplications(int recruitmentId)
        {
            var res = await _context.Applications
                .Where(a => a.RecruitmentId == recruitmentId)
                .ToListAsync();

            var dest = res.Adapt<List<ApplicationView>>();
            return dest;
        }

        private async Task ChangeRecruitmentStatus(int id, RecruitmentStatus status)
        {
            var recruitment = await _context.Recruitments.FindAsync(id);

            recruitment.Status = status;
            var res = _context.Recruitments.Update(recruitment);
            await _context.SaveChangesAsync();
        }

        private async Task<EntityEntry<Recruitment>> CopyAndSaveThumbnail(AddRecruitmentDto addRecruitmentDto,
            EntityEntry<Recruitment> res)
        {
            if (addRecruitmentDto.ImgUrl == ImgPath.PlaceHolder) return res;

            res.Entity.ImgUrl = MoveRecruitmentAndGetUrl(addRecruitmentDto, res);
            res = _context.Recruitments.Update(res.Entity);
            await _context.SaveChangesAsync();

            return res;
        }

        private string MoveRecruitmentAndGetUrl(AddRecruitmentDto addRecruitmentDto, EntityEntry<Recruitment> res)
        {
            if (addRecruitmentDto.ImgUrl.IsNullOrEmpty()) return ImgPath.PlaceHolder;

            try
            {
                var path = Path.Combine("Resources", "Images", "Thumbnails");
                return MoveAndGetUrl(addRecruitmentDto.ImgUrl, res.Entity.Id.ToString(), path, "Recruitment");
            }
            catch (IOException e)
            {
                _logger.LogInformation(e, "IOException, reverting to the default image");
                return ImgPath.PlaceHolder;
            }
        }

        private IQueryable<Recruitment> GetRecruitments(RecruitmentMode recruitmentMode, string userId = "")
        {
            return recruitmentMode switch
            {
                RecruitmentMode.Public => _context.Recruitments.Where(r => r.Status == RecruitmentStatus.Open),
                RecruitmentMode.Recruiter => _context.Recruitments.Where(r => r.RecruiterId == userId),
                RecruitmentMode.Admin => _context.Recruitments,
                _ => throw new ArgumentException("This mode doesn't exist")
            };
        }

        private async Task<EntityEntry<RecruitmentApplication>> CopyAndSaveApplicationFiles(
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

        private string MoveApplicationFileAndGetUrl(string parameter, int id, string subfolder)
        {
            if (parameter.IsNullOrEmpty()) return "";

            try
            {
                var path = Path.Combine("Resources", "Files", subfolder);
                return MoveAndGetUrl(parameter, id.ToString(), path, subfolder);
            }
            catch (IOException e)
            {
                _logger.LogInformation(e, "IOException, could not save" + subfolder);
                return "";
            }
        }

        private void GetDaysAgoDescriptions(ref List<RecruitmentView> dest)
        {
            dest.ForEach(dst => dst.DaysAgo = _dateTimeProvider.GetTimeAgoDescription(dst.StartDate));
        }
    }
}