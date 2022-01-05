using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DataModels;

public class CodeQualityAssessment
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public bool Success { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CompletedTime { get; set; }

    //Complexity
    public float? Complexity { get; set; }
    public float? CognitiveComplexity { get; set; }

    //Duplications
    public float? DuplicatedLines { get; set; }
    public float? DuplicatedLinesDensity { get; set; }

    //Issues
    public float? Violations { get; set; }

    //Maintability
    public float? CodeSmells { get; set; }
    public float? TechnicalDebt { get; set; }
    public float? MaintainabilityRating { get; set; }

    //Reliability
    public float? Bugs { get; set; }
    public float? ReliabilityRating { get; set; }

    //Tests
    public float? Coverage { get; set; }
    public float? Tests { get; set; }

    //Security
    public float? SecurityRating { get; set; }
    public float? SecurityHotspots { get; set; }

    //Size
    public float? LinesOfCode { get; set; }

    //Overall
    public float? OverallRating { get; set; }
}