using System;
using System.ComponentModel.DataAnnotations;
using Cars.Models.DataModels;

namespace Cars.Models.View
{
    public class ApplicationView
    {
        public int Id { get; set; }

        public RecruitmentApplication Recruitment { get; set; }

        public ApplicationUser Applicant { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
    }
}