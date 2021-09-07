using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Models.Dto;
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

        public async Task<List<RecruitmentView>>
            GetRecruitmentsFiltered(RecruitmentFilterDto recruitmentFilterDto) //TODO: FILTERS AND PAGINATION
        {
            var res = await _context.Recruitments.ToListAsync();
            var dest = res.Adapt<List<RecruitmentView>>();
            return dest;
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
    }
}