﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Managers.Interfaces;
using Cars.Models.DataModels;
using Cars.Models.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Cars.Managers.Implementations
{
    public class AppAppUserManager : IAppUserManager
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public AppAppUserManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> SetProfilePictureAsync(string userId, string profilePicture)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null) throw new ArgumentNullException(nameof(user));

            user.ProfilePicture = profilePicture;

            var res = _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<List<string>> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new KeyNotFoundException($"User {userId} not found");
            var roles = new List<string>();
            var all = new[] { "Admin", "Recruiter", "User" };
            foreach (var r in all)
            {
                var userIsInRole = await _userManager.IsInRoleAsync(user, r);
                if (userIsInRole) roles.Add(r);
            }
            return roles;
        }

        public string GetUserId(ClaimsPrincipal user)
        {
            return _userManager.GetUserId(user);
        }

        public string GetUserName(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        
        public async Task<ApplicationUser> FindUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new AppBaseException(HttpStatusCode.NotFound, $"User {userId} not found");
            return user;
        }

        public async Task<List<ApplicationUser>> GetFilteredUsers(string roleName, string searchTerm)
        {
            var res = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
            
            if (!string.IsNullOrEmpty(searchTerm))
                res = res.FindAll(r =>
                    r.Name.Contains(searchTerm) ||
                    r.Surname.Contains(searchTerm) ||
                    r.Email.Contains(searchTerm) ||
                    r.UserName.Contains(searchTerm));
            return res;
        }

        public void CheckIfRoleExists(string roleName)
        {
            if (!_context.Roles.Any(r => r.Name.Equals(roleName)))
                throw new AppBaseException(HttpStatusCode.NotFound, $"Role {roleName} not found");
        }
    }
}