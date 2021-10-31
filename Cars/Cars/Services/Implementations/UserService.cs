using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Services.Interfaces;
using Microsoft.AspNet.Identity;

namespace Cars.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> SetProfilePictureAsync(string userId, string profilePicture)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.ProfilePicture = profilePicture;
            
            var res = _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return res.Entity;
        }
    }
}