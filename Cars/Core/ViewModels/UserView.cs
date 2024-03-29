﻿using Core.DataModels;

namespace Core.ViewModels;

public class UserView
{
    public string? Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Description { get; set; }
    public string? ProfilePicture { get; set; }

    public City? City { get; set; }

    public string? Github { get; set; }
    public string? LinkedIn { get; set; }

    public List<Skill>? Skills { get; set; }
    public List<Education>? Education { get; set; }
    public List<Experience>? Experience { get; set; }
}