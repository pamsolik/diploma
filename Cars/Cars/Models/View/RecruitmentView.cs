﻿using System.ComponentModel.DataAnnotations;
using Cars.Models.Enums;

namespace Cars.Models.View
{
    public class RecruitmentView
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string ShortDescription { get; set; }
        
        public string City { get; set; }
        
        public string ImgUrl { get; set; } = ImgPath.PlaceHolder;
        
        //TODO: Calculate
        public int DaysAgo { get; set; }
    }
}