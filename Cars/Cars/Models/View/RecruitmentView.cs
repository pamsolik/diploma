using System.ComponentModel.DataAnnotations;
using Cars.Models.Enums;

namespace Cars.Models.View
{
    public class RecruitmentView
    {
        public int Id { get; set; }

        [StringLength(500, ErrorMessage = "Title cannot be longer than 500 characters.")]
        public string Title { get; set; }

        [StringLength(10000, ErrorMessage = "Description cannot be longer than 10000 characters.")]
        public string Description { get; set; }

        public string JobType { get; set; }
        public JobLevel JobLevel { get; set; }
    }
}