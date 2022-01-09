using Core.DataModels;

namespace Services.CodeQuality;

public static class OverallQualityCalculator
{
    public static float? CalculateOverallRating(CodeQualityAssessment? ass)
    {
        var ratings = new List<float?>
        {
            ass?.ReliabilityRating,
            ass?.MaintainabilityRating,
            ass?.SecurityRating
        };

        var complexity = new List<float?>
        {
            ass?.Complexity,
            ass?.CognitiveComplexity
        };

        var problems = new List<float?>
        {
            ass?.CodeSmells * 50,
            ass?.Violations * 250,
            ass?.Bugs * 500,
            ass?.SecurityHotspots * 1000
        };

        var ratingsAvg = ratings.Sum() * 3;

        var complexityAvg = complexity.Sum() / 50;

        var problemsAvg = problems.Sum() / ass?.LinesOfCode;

        var debt = ass?.TechnicalDebt / ass?.LinesOfCode * 50;

        return ratingsAvg + complexityAvg + problemsAvg + debt;
    }
}