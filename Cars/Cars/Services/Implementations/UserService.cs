using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.Exceptions;
using Cars.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cars.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<string> GetUserId(string userName)
        {
            var usr = await _context.Users.Where(user => user.UserName == userName).FirstOrDefaultAsync();
            return usr != null ? usr.Id : throw new AppBaseException( HttpStatusCode.NotFound,"User not found.");
        }
    }
}