﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cars.Models.DataModels;
using Cars.Models.Enums;

namespace Cars.Models.View
{
    public class RecruitmentDetailsView
    {
        public int Id { get; set; }

        public string RecruiterId { get; set; }

        public ApplicationUser Recruiter { get; set; }
        
        public string Title { get; set; }
        
        public string ShortDescription { get; set; }
        
        public string Description { get; set; }
        
        public DateTime StartDate { get; set; }

        public RecruitmentStatus Status { get; set; }

        public RecruitmentType Type { get; set; }
        
        public JobType JobType { get; set; }
        
        public JobLevel JobLevel { get; set; }
        
        public TeamSize TeamSize { get; set; }
        
        public string Field { get; set; }
        
        public string City { get; set; }
    }
}