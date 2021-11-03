using Cars.Models.DataModels;

namespace Cars.Models.View
{
    public class ProjectView
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int ApplicationId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public int? CodeQualityAssessmentId { get; set; }

        public virtual CodeQualityAssessment CodeQualityAssessment { get; set; }
    }
}