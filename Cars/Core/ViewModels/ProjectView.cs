using Core.DataModels;
using Core.Enums;

namespace Core.ViewModels;

public class ProjectView
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public int ApplicationId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public Technology Technology { get; set; }

    public int? CodeQualityAssessmentId { get; set; }

    public virtual CodeQualityAssessment? CodeQualityAssessment { get; set; }
}