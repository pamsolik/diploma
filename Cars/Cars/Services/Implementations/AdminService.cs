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
    public class AdminService : IAdminService
    {
       
        private readonly ApplicationDbContext _context;
        
        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }
        
    }
}