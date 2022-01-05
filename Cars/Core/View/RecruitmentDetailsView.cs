using Core.DataModels;
using Core.Enums;

namespace Core.View;

public class RecruitmentDetailsView
{
    public int Id { get; set; }

    public string RecruiterId { get; set; } = string.Empty;

    public UserView? Recruiter { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public RecruitmentStatus Status { get; set; }

    public RecruitmentType Type { get; set; }

    public JobType JobType { get; set; }

    public JobLevel JobLevel { get; set; }

    public TeamSize TeamSize { get; set; }

    public string Field { get; set; } = string.Empty;

    public City? City { get; set; }

    public string? ImgUrl { get; set; } = ImgPath.PlaceHolder;

    public string ClauseRequired { get; set; } = string.Empty;

    public string ClauseOpt1 { get; set; } = string.Empty;

    public string ClauseOpt2 { get; set; } = string.Empty;
}