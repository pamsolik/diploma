using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cars.Models.DataModels
{
    public class CodeOverallQuality : CodeQualityAssessment
    {
        public int ProjectsCount { get; set; }
    }
}