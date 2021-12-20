using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cars.Models.DataModels
{
    public class Education
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "SmallDescription cannot be longer than 100 characters.")]
        public string Title { get; set; }

        [Required] public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")] public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "SmallDescription cannot be longer than 100 characters.")]
        public string SmallDescription { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "SmallDescription cannot be longer than 5000 characters.")]
        public string Description { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string DateFrom { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string DateTo { get; set; }
    }
}