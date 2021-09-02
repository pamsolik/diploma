using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Cars.Data;
using Cars.Models;
using Cars.Models.Dto;
using Cars.Services.Interfaces;
using Mapster;
using Cars.Models.View;
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
            var res =  _context.Recruitments.Update(dest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<RecruitmentView>> GetRecruitments()
        {
            var res = await _context.Recruitments.ToListAsync();
            var dest = res.Adapt<List<RecruitmentView>>();
            return dest;
        }
        
        public async Task<List<RecruitmentView>> GetRecruitmentsFiltered(RecruitmentFilterDto recruitmentFilterDto) //TODO: FILTERS AND PAGINATION
        {
            var res = await _context.Recruitments.ToListAsync();
            var dest = res.Adapt<List<RecruitmentView>>();
            return dest;
        }

        public RecruitmentView Test(){
            var src = _context.Recruitments.FirstOrDefault();
            var destinationObject = (src ?? throw new InvalidOperationException()).Adapt<RecruitmentView>();
            return destinationObject;
        }
    }
}