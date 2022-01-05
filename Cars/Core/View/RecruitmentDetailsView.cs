using Core.DataModels;
using Core.Enums;

namespace Core.View;

public class RecruitmentDetailsView
{
    public int Id { get; set; }

    public string RecruiterId { get; set; }

    public UserView Recruiter { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public RecruitmentStatus Status { get; set; }

    public RecruitmentType Type { get; set; }

    public JobType JobType { get; set; }

    public JobLevel JobLevel { get; set; }

    public TeamSize TeamSize { get; set; }

    public string Field { get; set; }

    public City City { get; set; }

    public string ImgUrl { get; set; } = ImgPath.PlaceHolder;

    public string ClauseRequired { get; set; }

    public string ClauseOpt1 { get; set; }

    public string ClauseOpt2 { get; set; }
}