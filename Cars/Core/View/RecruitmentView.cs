using Core.DataModels;

namespace Core.View;

public class RecruitmentView
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public City? City { get; set; }

    public string ImgUrl { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public string DaysAgo { get; set; } = string.Empty;
}