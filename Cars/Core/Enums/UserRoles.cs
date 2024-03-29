﻿using System.ComponentModel.DataAnnotations;

namespace Core.Enums;

public enum UserRoles
{
    [Display(Name = "User")] User = 0,

    [Display(Name = "Recruiter")] Recruiter = 1,

    [Display(Name = "Admin")] Admin = 2
}