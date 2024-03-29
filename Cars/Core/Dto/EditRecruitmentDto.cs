﻿using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Dto;

public class EditRecruitmentDto
{
    public int Id { get; set; }

    [Required] public string? RecruiterId { get; set; }

    [Required]
    [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500, ErrorMessage = "Short description cannot be longer than 500 characters.")]
    public string ShortDescription { get; set; } = string.Empty;

    [Required]
    [StringLength(10000, ErrorMessage = "Description cannot be longer than 10000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required] public RecruitmentStatus Status { get; set; }

    [Required] public RecruitmentType Type { get; set; }

    [Required] public JobType JobType { get; set; }

    [Required] public JobLevel JobLevel { get; set; }

    [Required] public TeamSize TeamSize { get; set; }

    [Required]
    [StringLength(200, ErrorMessage = "Field cannot be longer than 200 characters.")]
    public string Field { get; set; } = string.Empty;

    [Required] public CityDto? City { get; set; }

    [Required] public string? ImgUrl { get; set; }

    [Required]
    [StringLength(1000, ErrorMessage = "ClauseRequired cannot be longer than 1000 characters.")]
    public string ClauseRequired { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "ClauseOpt1 cannot be longer than 1000 characters.")]
    public string ClauseOpt1 { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "ClauseOpt2 cannot be longer than 1000 characters.")]
    public string ClauseOpt2 { get; set; } = string.Empty;
}