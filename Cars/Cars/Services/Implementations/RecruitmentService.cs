using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Models.Dto;
using Cars.Models.Enums;
using Cars.Models.View;
using Cars.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Cars.Services.Implementations
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly ApplicationDbContext _context;

        public RecruitmentService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<bool> AddRecruitment(AddRecruitmentDto addRecruitmentDto)
        {
            var dest = addRecruitmentDto.Adapt<Recruitment>(); //TODO: check if valid and respond accordingly
            var res = _context.Recruitments.Add(dest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditRecruitment(EditRecruitmentDto addRecruitmentDto)
        {
            var dest = addRecruitmentDto.Adapt<Recruitment>(); //TODO: check if valid and respond accordingly
            var res = _context.Recruitments.Update(dest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<RecruitmentView>> GetRecruitments()
        {
            var res = await _context.Recruitments.ToListAsync();
            var dest = res.Adapt<List<RecruitmentView>>();
            return dest;
        }

        public async Task<RecruitmentDetailsView> GetRecruitmentDetails(int recruitmentId)
        {
            var res = await _context.Recruitments.FindAsync(recruitmentId);
            var dest = res.Adapt<RecruitmentDetailsView>();
            return dest;
        }

        public async Task<PaginatedList<RecruitmentView>>
            GetRecruitmentsFiltered(RecruitmentFilterDto filter)
        {
            var recruitments = _context.Recruitments.AsQueryable()
                .Where(r => r.Status == RecruitmentStatus.Open);

            recruitments = FilterOutAndSortRecruitments(ref recruitments, filter);

            var recruitmentList = await recruitments.ToListAsync();
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
        
        private static IQueryable<Recruitment> FilterOutAndSortRecruitments(ref IQueryable<Recruitment> recruitments,
            RecruitmentFilterDto filter)
        {
            if (!string.IsNullOrEmpty(filter.SearchString))
                recruitments = recruitments.Where(s => s.Title.Contains(filter.SearchString)
                                                       || s.Description.Contains(filter.SearchString));
            //TODO: Rest of the filters


            recruitments = filter.SortOrder switch
            {
                SortOrder.NameAsc => recruitments.OrderBy(s => s.Title),
                SortOrder.NameDesc => recruitments.OrderByDescending(s => s.Title),
                SortOrder.DateAddedAsc => recruitments.OrderBy(s => s.StartDate),
                SortOrder.DateAddedDesc => recruitments.OrderByDescending(s => s.StartDate),
                //TODO: Order by closest
                _ => recruitments.OrderBy(s => s.Title)
            };

            return recruitments;
        }

        // async Task<PaginatedList<RecruitmentView>> ConvertToPaginatedList()
        // {
        //     //Maybe in the future
        // }
    }
}