using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cars.Models.DataModels
{
    public class CodeQualityAssessment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool Success { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CompletedTime { get; set; }

        
        //All metrics from sonar +
        //code lenght +
        //some errors/lenght metric to evaluate the code
    }
}