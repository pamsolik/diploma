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
            var dest = addRecruitmentDto.Adapt<Recruitment>();
            var res = await _context.Recruitments.AddAsync(dest);
            return true;
        }

        public RecruitmentView Test(){
            var src = _context.Recruitments.FirstOrDefault();
            var destinationObject = (src ?? throw new InvalidOperationException()).Adapt<RecruitmentView>();
            return destinationObject;
        }
    }
}