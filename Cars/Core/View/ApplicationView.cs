using Core.DataModels;
using Core.Enums;

namespace Core.View;

public class ApplicationView
{
    public int Id { get; set; }

    public UserView? Applicant { get; set; }

    public string Description { get; set; } = string.Empty;

    public string CvFile { get; set; } = string.Empty;

    public string ClFile { get; set; } = string.Empty;

    public List<ProjectView>? Projects { get; set; }

    public DateTime Time { get; set; }

    public ClauseAccept ClauseOptAccepted { get; set; }

    public ClauseAccept ClauseOpt2Accepted { get; set; }

    public CodeOverallQuality? CodeOverallQuality { get; set; }

    public bool Selected { get; set; }
}