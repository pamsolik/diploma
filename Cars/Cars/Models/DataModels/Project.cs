﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cars.Models.Enums;

namespace Cars.Models.DataModels
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        public string Description { get; set; }

        [Required] public int ApplicationId { get; set; }

        [ForeignKey("ApplicationId")] public virtual RecruitmentApplication Application { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Url cannot be longer than 1000 characters.")]
        public string Url { get; set; }

        [Required] public Technology Technology { get; set; }

        public int Retries { get; set; }

        public int SolutionsCnt { get; set; } = 1;

        //null if not calculated yet
        public int? CodeQualityAssessmentId { get; set; }

        [ForeignKey("CodeQualityAssessmentId")]
        public virtual CodeQualityAssessment CodeQualityAssessment { get; set; }
    }
}