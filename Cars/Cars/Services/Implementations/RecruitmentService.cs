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
using IdentityServer4.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Cars.Services.Implementations
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly ApplicationDbContext _context;

        public RecruitmentService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<int> AddRecruitment(AddRecruitmentDto addRecruitmentDto, string recruiterId)
        {
             //TODO: check if valid and respond accordingly
            var existingCity = _context.Cites.FirstOrDefault(CompareCities(addRecruitmentDto));
            
            if (existingCity is null)
            {
                existingCity = addRecruitmentDto.City.Adapt<City>();
                _context.Cites.Add(existingCity);
            }
            
            var dest = addRecruitmentDto.Adapt<Recruitment>();
            
            dest.City = existingCity;
            dest.RecruiterId = recruiterId;
            
            var res = _context.Recruitments.Add(dest);
            await _context.SaveChangesAsync();
            
            res = await CopyAndSaveThumbnail(addRecruitmentDto, res);
            return res.Entity.Id;
        }

        private async Task<EntityEntry<Recruitment>> CopyAndSaveThumbnail(AddRecruitmentDto addRecruitmentDto, EntityEntry<Recruitment> res)
        {
            if (addRecruitmentDto.ImgUrl.IsNullOrEmpty())
            {
                res.Entity.ImgUrl = ImgPath.PlaceHolder;
                res = _context.Recruitments.Update(res.Entity);
                await _context.SaveChangesAsync();
            }
            else if (addRecruitmentDto.ImgUrl != ImgPath.PlaceHolder)
            {
                try
                {
                    var basePath = Directory.GetCurrentDirectory();
                    var ext = Path.GetExtension(addRecruitmentDto.ImgUrl);
                    var imgUrl = Path.Combine("Resources", "Images", "Thumbnails", $"Recruitment_{res.Entity.Id}{ext}");
                    File.Move(Path.Combine(basePath, addRecruitmentDto.ImgUrl ?? throw new FileNotFoundException()),
                        Path.Combine(basePath, imgUrl), true);
                    res.Entity.ImgUrl = imgUrl;
                }
                catch (IOException e)
                {
                    res.Entity.ImgUrl = ImgPath.PlaceHolder;
                } 
                res = _context.Recruitments.Update(res.Entity);
                await _context.SaveChangesAsync();
            }
            return res;
        }

        private static Expression<Func<City, bool>> CompareCities(AddRecruitmentDto addRecruitmentDto)
        {
            //TODO: Refactor
            return c => c.Name == addRecruitmentDto.City.Name && 
                        c.Latitude == addRecruitmentDto.City.Latitude && 
                        c.Longitude == addRecruitmentDto.City.Longitude;
        }

        public async Task<bool> EditRecruitment(EditRecruitmentDto addRecruitmentDto)
        {
            var recruitment = await _context.Recruitments.FindAsync(addRecruitmentDto.Id); //TODO 
            var dest = addRecruitmentDto.Adapt<Recruitment>(); //TODO: check if valid and respond accordingly
            var res = _context.Recruitments.Update(dest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<RecruitmentView>> GetRecruitments(string userId)
        {
            var res = await _context.Recruitments.Where(r => r.RecruiterId == userId).ToListAsync();
            var dest = res.Adapt<List<RecruitmentView>>();
            return dest;
        }

        public async Task<RecruitmentDetailsView> GetRecruitmentDetails(int recruitmentId)
        {
            var res = await _context.Recruitments.FindAsync(recruitmentId);
            res.City = await _context.Cites.FindAsync(res.CityId); //TODO: FIX
            var dest = res.Adapt<RecruitmentDetailsView>();
            return dest;
        }

        public async Task<PaginatedList<RecruitmentView>>
            GetRecruitmentsFiltered(RecruitmentFilterDto filter)
        {
            var recruitments = _context.Recruitments
                .Where(r => r.Status == RecruitmentStatus.Open);
            
            var recruitmentList = FilterOutAndSortRecruitments(ref recruitments, filter).ToList();
            var dest = recruitmentList.Adapt<List<RecruitmentView>>();
            var paginated = PaginatedList<RecruitmentView>.CreateAsync(dest, filter.PageIndex, filter.PageSize);

            return paginated;
        }

        public async Task<bool> AddApplication(AddApplicationDto addApplicationDto)
        {
            var dest = addApplicationDto.Adapt<RecruitmentApplication>(); //TODO: check if valid and respond accordingly
            var res = _context.Applications.Add(dest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ApplicationView>> GetApplications(int recruitmentId)
        {
            var res = await _context.Applications
                .Where(a => a.RecruitmentId == recruitmentId).ToListAsync();
            var dest = res.Adapt<List<ApplicationView>>();
            return dest;
        }

        private static IEnumerable<Recruitment> FilterOutAndSortRecruitments(ref IQueryable<Recruitment> recruitments,
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
                //TODO: Order by closest
                _ => recruitments.OrderBy(s => s.Title)
            };

            return filtered;
        }

        private double CalculateDistance(Recruitment recruitment, double latitude, double longitude)
        {
            var sCoord = new GeoCoordinate(recruitment.City.Latitude, recruitment.City.Longitude);
            var eCoord = new GeoCoordinate(latitude, longitude);
            return sCoord.GetDistanceTo(eCoord);
        }

        private static void FilterPickedValues(ref IEnumerable<Recruitment> list,
            IReadOnlyList<bool?> filter,
            Func<Recruitment, int> predicate)
        {
            if (filter.Any(f => f == true))
            {
                list = list.Where(li =>
                {
                    var val = predicate(li);
                    return filter.Count > val && filter[val] == true;
                });
            }
        }

        // async Task<PaginatedList<RecruitmentView>> ConvertToPaginatedList()
        // {
        //     //Maybe in the future
        // }
    }
}