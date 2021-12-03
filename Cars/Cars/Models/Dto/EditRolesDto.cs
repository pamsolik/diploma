using System.ComponentModel.DataAnnotations;

namespace Cars.Models.Dto
{
    public class EditRolesDto
    {
        [Required] public string UserId { get; set; }

        [Required] public string Role { get; set; }
    }
}