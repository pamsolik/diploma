using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DataModels;

public class Skill
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public string ApplicationUserId { get; set; }

    [ForeignKey("ApplicationUserId")] public virtual ApplicationUser ApplicationUser { get; set; }

    [Required]
    [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
    public string Description { get; set; }
}