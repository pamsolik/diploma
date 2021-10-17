using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cars.Models.DataModels
{
    public class Project : TextModel
    {
        [Required]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        public string Title { get; set; }
        
        [Required]
        [StringLength(1000, ErrorMessage = "Url cannot be longer than 1000 characters.")]
        public string Url { get; set; }
    }
}