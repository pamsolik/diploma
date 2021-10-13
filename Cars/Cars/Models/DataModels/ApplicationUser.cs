using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Cars.Models.DataModels
{
    public class ApplicationUser : IdentityUser
    {
        //TODO: Add user properties to the ui
        
        public string Name { get; set; }
        public string Surname { get; set; }
        
        public string Description { get; set; }
        
        public string ProfilePicture { get; set; }
        
        public City City { get; set; }
        
        public string Github { get; set; }
        public string LinkedIn { get; set; }
        
        public List<string> Skills { get; set; }
        
        public List<Experience> Education { get; set; }
        public List<Experience> Experience { get; set; }
    }
}