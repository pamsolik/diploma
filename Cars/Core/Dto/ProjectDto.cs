using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Dto;

public class ProjectDto
{
    [Required]
    [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, ErrorMessage = "Url cannot be longer than 1000 characters.")]
    public string Url { get; set; } = string.Empty;

    [Required] public Technology Technology { get; set; }
}