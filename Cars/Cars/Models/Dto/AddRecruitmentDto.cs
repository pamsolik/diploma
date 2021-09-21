using System;
using System.ComponentModel.DataAnnotations;
using Cars.Models.Enums;

namespace Cars.Models.Dto
{
    public class AddRecruitmentDto
    {
        [Required]
        public string RecruiterId { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        public string Title { get; set; }

        [Required] 
        [StringLength(500, ErrorMessage = "Short description cannot be longer than 500 characters.")]
        public string ShortDescription { get; set; }
        
        [Required]
        [StringLength(10000, ErrorMessage = "Description cannot be longer than 10000 characters.")]
        public string Description { get; set; }
        
        [Required] public RecruitmentStatus Status { get; set; }

        [Required] public RecruitmentType Type { get; set; }
        
        [Required] public JobType JobType { get; set; }

        [Required] public JobLevel JobLevel { get; set; }

        [Required]
        public TeamSize TeamSize { get; set; }
        
        [Required]
        [StringLength(200, ErrorMessage = "Field cannot be longer than 200 characters.")]
        public string Field { get; set; }
        
        [Required]
        [StringLength(1000, ErrorMessage = "City cannot be longer than 1000 characters.")]
        public string City { get; set; }
    }
}