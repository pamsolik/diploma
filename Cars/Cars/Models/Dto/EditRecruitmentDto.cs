using System;
using System.ComponentModel.DataAnnotations;
using Cars.Models.Enums;

namespace Cars.Models.Dto
{
    public class EditRecruitmentDto
    {
        [Required] public int Id { get; set; }

        [StringLength(500, ErrorMessage = "Title cannot be longer than 500 characters.")]
        public string Title { get; set; }

        [StringLength(10000, ErrorMessage = "Description cannot be longer than 10000 characters.")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public RecruitmentType Type { get; set; }

        public string JobType { get; set; }
        public JobLevel JobLevel { get; set; }

        public string RecruiterId { get; set; }
    }
}