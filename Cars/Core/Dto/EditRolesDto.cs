using System.ComponentModel.DataAnnotations;

namespace Core.Dto;

public class EditRolesDto
{
    [Required] public string UserId { get; set; }

    [Required] public string Role { get; set; }
}