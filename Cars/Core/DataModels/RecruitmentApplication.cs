﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;

namespace Core.DataModels;

public class RecruitmentApplication
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public int RecruitmentId { get; set; }

    [ForeignKey("RecruitmentId")] public virtual Recruitment? Recruitment { get; set; }

    [Required] public string? ApplicantId { get; set; }

    [ForeignKey("ApplicantId")] public virtual ApplicationUser? Applicant { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
    public string? Description { get; set; }

    public string? CvFile { get; set; }

    public string? ClFile { get; set; }

    public virtual ICollection<Project>? Projects { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Time { get; set; }

    public ClauseAccept ClauseOptAccepted { get; set; }

    public ClauseAccept ClauseOpt2Accepted { get; set; }

    //null if not calculated yet
    public int? CodeOverallQualityId { get; set; }

    [ForeignKey("CodeOverallQualityId")] public virtual CodeOverallQuality? CodeOverallQuality { get; set; }

    public bool Selected { get; set; } = false;
}