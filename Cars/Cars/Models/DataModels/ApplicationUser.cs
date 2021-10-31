﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Cars.Models.DataModels
{
    public class ApplicationUser : IdentityUser
    {
        //TODO: Add user properties to the ui
        
        [StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters.")]
        public string Name { get; set; }
        
        [StringLength(200, ErrorMessage = "Surname cannot be longer than 200 characters.")]
        public string Surname { get; set; }

        
        [StringLength(2000, ErrorMessage = "Description cannot be longer than 2000 characters.")]
        public string Description { get; set; }

        [StringLength(200, ErrorMessage = "ProfilePicture path cannot be longer than 200 characters.")]
        public string ProfilePicture { get; set; }

        public virtual City City { get; set; }

        
        [StringLength(200, ErrorMessage = "Github link cannot be longer than 200 characters.")]
        public string Github { get; set; }
        
        [StringLength(200, ErrorMessage = "LinkedIn link cannot be longer than 200 characters.")]
        public string LinkedIn { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }

        public virtual ICollection<Education> Education { get; set; }
        public virtual ICollection<Experience> Experience { get; set; }
    }
}