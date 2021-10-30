using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Models.Dto;
using Cars.Models.Enums;
using Cars.Models.View;
using Cars.Services.Interfaces;
using Cars.Services.Other;
using IdentityServer4.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using static Cars.Services.Other.FileService;

namespace Cars.Services.Implementations
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly ILogger<RecruitmentService> _logger;
        private readonly ApplicationDbContext _context;

        private readonly IDateTimeProvider _dateTimeProvider;

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

        public async Task<RecruitmentDetailsView> GetRecruitmentDetails(int recruitmentId)
        {
            var res = await _context.Recruitments.FindAsync(recruitmentId);
            res.City = await _context.Cities.FindAsync(res.CityId); //TODO: FIX
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
            return res.Entity;
        }

        public async Task<List<ApplicationView>> GetApplications(int recruitmentId)
        {
            var res = await _context.Applications
                .Where(a => a.RecruitmentId == recruitmentId)
                .ToListAsync();

            res.ForEach(app =>
            {
                app.Applicant = _context.ApplicationUsers.FirstOrDefault(u => u.Id == app.ApplicantId);
                app.Projects = _context.Projects.Where(p => app.Id == p.ApplicationId).ToList();

                if (app.Projects == null) return;
                foreach (var p in app.Projects)
                {
                    p.CodeQualityAssessment = _context.CodeQualityAssessments
                        .FirstOrDefault(q => q.Id == p.CodeQualityAssessmentId);
                }
            });

            var dest = res.Adapt<List<ApplicationView>>();
            return dest;
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
                return MoveAndGetUrl(addRecruitmentDto.ImgUrl, res.Entity.Id, path, "Recruitment");
            }
            catch (IOException e)
            {
                _logger.LogInformation(e, "IOException, reverting to the default image");
                return ImgPath.PlaceHolder;
            }
        }

        private static Expression<Func<City, bool>> CompareCities(AddRecruitmentDto addRecruitmentDto)
        {
            //TODO: Refactor
            return c => c.Name == addRecruitmentDto.City.Name &&
                        c.Latitude == addRecruitmentDto.City.Latitude &&
                        c.Longitude == addRecruitmentDto.City.Longitude;
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

        private static IEnumerable<Recruitment> FilterOutAndSortRecruitments(
            ref IQueryable<Recruitment> recruitments,
            RecruitmentFilterDto filter)
        {
            if (!string.IsNullOrEmpty(filter.SearchString))
                recruitments = recruitments.Where(s =>
                    s.Title.Contains(filter.SearchString) || s.Description.Contains(filter.SearchString));

            var filtered = recruitments.AsEnumerable();

            FilterPickedValues(ref filtered, filter.JobLevels, x => (int)x.JobLevel);
            FilterPickedValues(ref filtered, filter.JobTypes, x => (int)x.JobType);
            FilterPickedValues(ref filtered, filter.TeamSizes, x => (int)x.TeamSize);

            filtered = filter.SortOrder switch
            {
                SortOrder.NameAsc => filtered.OrderBy(s => s.Title),
                SortOrder.NameDesc => filtered.OrderByDescending(s => s.Title),
                SortOrder.DateAddedAsc => filtered.OrderBy(s => s.StartDate),
                SortOrder.DateAddedDesc => filtered.OrderByDescending(s => s.StartDate),
                SortOrder.Closest => filtered.OrderByDescending(s =>
                    CalculateDistance(s, filter.City.Latitude, filter.City.Longitude)),
                _ => recruitments.OrderBy(s => s.Title)
            };

            return filtered;
        }

        private static double CalculateDistance(Recruitment recruitment, double latitude, double longitude)
        {
            var sCoord = new GeoCoordinate(recruitment.City.Latitude, recruitment.City.Longitude);
            var eCoord = new GeoCoordinate(latitude, longitude);
            return sCoord.GetDistanceTo(eCoord);
        }

        private static void FilterPickedValues(ref IEnumerable<Recruitment> list,
            IReadOnlyList<bool?> filter, Func<Recruitment, int> predicate)
        {
            if (filter.Any(f => f == true))
                list = list.Where(li =>
                {
                    var val = predicate(li);
                    return filter.Count > val && filter[val] == true;
                });
        }

        public void GetDaysAgoDescriptions(ref List<RecruitmentView> dest)
        {
            dest.ForEach(dst => dst.DaysAgo = _dateTimeProvider.GetTimeAgoDescription(dst.StartDate));
        }
    }
}