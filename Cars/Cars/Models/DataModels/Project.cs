using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        
        //null if not calculated yet
        public int? CodeQualityAssessmentId { get; set; }

        [ForeignKey("CodeQualityAssessmentId")]
        public virtual CodeQualityAssessment CodeQualityAssessment { get; set; }
    }
}