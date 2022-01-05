using System.ComponentModel.DataAnnotations;

namespace Core.Dto;

public class CityDto
{
    [StringLength(1000, ErrorMessage = "City Name cannot be longer than 1000 characters.")]
    public string Name { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}