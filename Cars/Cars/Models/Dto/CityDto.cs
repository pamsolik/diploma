using System.ComponentModel.DataAnnotations;

namespace Cars.Models.Dto
{
    public class CityDto
    {
        [StringLength(1000, ErrorMessage = "City Name cannot be longer than 1000 characters.")]
        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}