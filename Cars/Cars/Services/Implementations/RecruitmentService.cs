using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


using Cars.Data;
using Cars.Models;
using Cars.Models.Dto;
using Cars.Services.Interfaces;
using Mapster;

namespace Cars.Services.Implementations
{
    public class RecruitmentService : IRecruitmentService
    {
       
        private readonly ApplicationDbContext _context;


        public RecruitmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public RecruitmentDto Test(){
            var src = _context.Recruitments.FirstOrDefault();
            var destinationObject = (src ?? throw new InvalidOperationException()).Adapt<RecruitmentDto>();
            return destinationObject;
        }
    }
}