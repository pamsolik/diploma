using System.Collections.Generic;
using System.Linq;
using Cars.Models.DataModels;

namespace Cars.Services.Other
{
    public static class OverallQualityCalculator
    {
        public static float CalculateOverallRating(CodeQualityAssessment ass)
        {
            var ratings = new List<float?>
            {
                ass.ReliabilityRating,
                ass.MaintainabilityRating,
                ass.SecurityRating
            };

            var complexity = new List<float?>
            {
                ass.Complexity,
                ass.CognitiveComplexity
            };

            var problems = new List<float?>
            {
                ass.CodeSmells * 50,
                ass.Violations * 250,
                ass.Bugs * 500,
                ass.SecurityHotspots * 1000
            };

            var ratingsAvg = ratings.Sum().GetValueOrDefault(0) * 3;

            var complexityAvg = complexity.Sum().GetValueOrDefault(0) / 50;

            var problemsAvg = (problems.Sum() / ass.LinesOfCode).GetValueOrDefault(0);

            var debt = (ass.TechnicalDebt / ass.LinesOfCode).GetValueOrDefault(0) * 50;

            return ratingsAvg + complexityAvg + problemsAvg + debt;
        }
    }
}