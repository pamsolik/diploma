using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.View;
using Cars.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Cars.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecruitmentView>> GetRecruitments()
        {
            var res = await _context.Recruitments.ToListAsync();
            var dest = res.Adapt<List<RecruitmentView>>();
            return dest;
        }
    }
}