using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cars.Models.Dto
{
    public class AddApplicationDto
    {
        //TODO: Add documents etc
        
        [Required]
        public int RecruitmentId { get; set; }

        [Required]
        public string ApplicantId { get; set; }

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