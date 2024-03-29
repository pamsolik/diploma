﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DataModels;

public class City
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(1000, ErrorMessage = "City Name cannot be longer than 1000 characters.")]
    public string? Name { get; set; }

    [Required] public double Latitude { get; set; }

    [Required] public double Longitude { get; set; }
}