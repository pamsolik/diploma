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

        public float? CodeSmells { get; set; }

        public float? Maintainability { get; set; }
        public float? Coverage { get; set; }
        public float? CognitiveComplexity { get; set; }
        public float? Violations { get; set; }
        public float? SecurityRating { get; set; }
        public float? DuplicatedLines { get; set; }
        public float? Lines { get; set; }
        public float? DuplicatedLinesDensity { get; set; }
        public float? Bugs { get; set; }
        public float? SqaleRating { get; set; }
        public float? ReliabilityRating { get; set; }
        public float? Complexity { get; set; }
        public float? SecurityHotspots { get; set; }

        public float? OverallRating { get; set; }
    }
}