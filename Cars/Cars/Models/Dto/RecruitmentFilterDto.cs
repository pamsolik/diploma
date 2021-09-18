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

        public SortOrder SortOrder { get; set; } = SortOrder.NameAsc;
    }
}