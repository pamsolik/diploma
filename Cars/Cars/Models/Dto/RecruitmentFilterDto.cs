using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cars.Models.Enums;

namespace Cars.Models.Dto
{
    public class RecruitmentFilterDto
    {
        [Required] public int PageSize { get; set; }

        [Required] public int PageIndex { get; set; } = 0;

        public string SearchString { get; set; }

        public List<JobLevel> JobLevels { get; set; }

        public List<JobType> JobTypes { get; set; }
        
        public List<TeamSize> TeamSizes { get; set; }
        
        public string City { get; set; }

        public int Distance { get; set; } = 0;
        public SortOrder SortOrder { get; set; } = SortOrder.NameAsc;
    }
}