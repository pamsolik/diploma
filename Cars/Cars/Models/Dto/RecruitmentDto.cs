using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cars.Models.Enums;

namespace Cars.Models.Dto
{
    public class RecruitmentDto
    {
        //TODO: Add recruitment properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(500, ErrorMessage = "Title cannot be longer than 500 characters.")]
        public string Title {get; set;}

        [StringLength(10000, ErrorMessage = "Description cannot be longer than 10000 characters.")]
        public string Description {get; set;}

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public RecruitmentType Type { get; set; }

        //TODO: Maybe change to enum
        public String JobType { get; set; }
        public JobLevel JobLevel { get; set; }


        [ForeignKey("ApplicationID")]
        public ICollection<RecruitmentApplication> Applications { get; set; }


        [ForeignKey("Recruiter")]
        public string RecruiterId { get; set; }
        public ApplicationUser Recruiter { get; set; }
    }
}
