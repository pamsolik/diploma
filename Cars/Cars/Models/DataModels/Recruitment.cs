using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cars.Models.Enums;

namespace Cars.Models.DataModels
{
    public class Recruitment
    {
        //TODO: Add recruitment properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Recruiter")] public string RecruiterId { get; set; }

        public ApplicationUser Recruiter { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Title cannot be longer than 500 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "Description cannot be longer than 10000 characters.")]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required] public RecruitmentStatus Status { get; set; }

        [Required] public RecruitmentType Type { get; set; }

        //TODO: Maybe change to enum
        [Required] public string JobType { get; set; }

        [Required] public JobLevel JobLevel { get; set; }

        public ICollection<RecruitmentApplication> Applications { get; set; }
    }
}