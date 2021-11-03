using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cars.Models.Dto
{
    public class AddApplicationDto
    {
        [Required] public int RecruitmentId { get; set; }

        public string Description { get; set; }

        public string CvFile { get; set; }

        public string ClFile { get; set; }

        public List<ProjectDto> Projects { get; set; }

        public bool ClauseOptAccepted { get; set; }

        public bool ClauseOpt2Accepted { get; set; }
    }
}