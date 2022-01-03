using System.ComponentModel.DataAnnotations;
using Cars.Models.Enums;

namespace Cars.Models.Dto;

public class ProjectDto
{
    [Required]
    [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
    public string Title { get; set; }

    [Required]
    [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
    public string Description { get; set; }

    [Required]
    [StringLength(1000, ErrorMessage = "Url cannot be longer than 1000 characters.")]
    public string Url { get; set; }

    [Required] public Technology Technology { get; set; }
}