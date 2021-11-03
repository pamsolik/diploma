using System;
using Cars.Models.DataModels;

namespace Cars.Models.View
{
    public class RecruitmentView
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public City City { get; set; }

        public string ImgUrl { get; set; }

        public DateTime StartDate { get; set; }
        public string DaysAgo { get; set; }
    }
}