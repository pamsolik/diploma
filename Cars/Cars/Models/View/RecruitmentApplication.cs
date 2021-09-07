using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cars.Models.View
{
    public class ApplicationView
    {
        public int Id { get; set; }
        
        [Required]
        public RecruitmentApplication Recruitment { get; set; }
        
        [Required]
        public ApplicationUser Applicant { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Title cannot be longer than 500 characters.")]
        public string Title {get; set;}

        [Required]
        [StringLength(10000, ErrorMessage = "Description cannot be longer than 10000 characters.")]
        public string Description {get; set;}

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }
        
    }
}
