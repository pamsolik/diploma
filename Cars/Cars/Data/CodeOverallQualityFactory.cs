using System.Collections.Generic;
using System.Linq;
using Cars.Models.DataModels;

namespace Cars.Data
{
    public class CodeOverallQualityFactory
    {
        public static CodeOverallQuality GetCodeOverallQuality(List<Project> projects)
        {
            switch (projects.Count)
            {
                case 0:
                    return null;
                case 1:
                    var q = projects.First().CodeQualityAssessment;
                    return new CodeOverallQuality
                    {
                        Success = true,
                        ProjectsCount = 1,
                        CompletedTime = q.CompletedTime,
                        CodeSmells = q.CodeSmells,
                        Maintainability = q.Maintainability,
                        Coverage = q.Coverage,
                        CognitiveComplexity = q.CognitiveComplexity,
                        Violations = q.Violations,
                        SecurityRating = q.SecurityRating,
                        DuplicatedLines = q.DuplicatedLines,
                        Lines = q.Lines,
                        DuplicatedLinesDensity = q.DuplicatedLinesDensity,
                        Bugs = q.Bugs,
                        SqaleRating = q.SqaleRating,
                        ReliabilityRating = q.ReliabilityRating,
                        Complexity = q.Complexity,
                        SecurityHotspots = q.SecurityHotspots,
                        OverallRating = q.OverallRating
                    };
                default:
                    return null;
            }
        }
    }
}