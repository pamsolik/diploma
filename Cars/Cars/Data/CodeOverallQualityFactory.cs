using System;
using System.Collections.Generic;
using System.Linq;
using Cars.Models.DataModels;
using Cars.Services.Interfaces;

namespace Cars.Data
{
    public static class CodeOverallQualityFactory
    {
        public static CodeOverallQuality GetCodeOverallQuality(List<Project> projects, IDateTimeProvider dateTimeProvider)
        {
            switch (projects.Count)
            {
                case 0:
                    return null;
                case 1:
                {
                    var p = projects.First().CodeQualityAssessment;
                    return new CodeOverallQuality
                    {
                        Success = true,
                        ProjectsCount = 1,
                        CompletedTime = dateTimeProvider.GetTimeNow(),
                        CodeSmells = p.CodeSmells,
                        MaintainabilityRating = p.MaintainabilityRating,
                        Coverage = p.Coverage,
                        CognitiveComplexity = p.CognitiveComplexity,
                        Violations = p.Violations,
                        SecurityRating = p.SecurityRating,
                        DuplicatedLines = p.DuplicatedLines,
                        LinesOfCode = p.LinesOfCode,
                        DuplicatedLinesDensity = p.DuplicatedLinesDensity,
                        Bugs = p.Bugs,
                        TechnicalDebt = p.TechnicalDebt,
                        ReliabilityRating = p.ReliabilityRating,
                        Complexity = p.Complexity,
                        SecurityHotspots = p.SecurityHotspots,
                        OverallRating = p.OverallRating
                    };
                }
                default:
                {
                    var p = projects.Select(a => a.CodeQualityAssessment).ToList();
                    return new CodeOverallQuality
                    {
                        Success = true,
                        ProjectsCount = p.Count,
                        CompletedTime = dateTimeProvider.GetTimeNow(),
                        CodeSmells = p.Sum(x => x.CodeSmells),
                        MaintainabilityRating = p.Average(x => x.MaintainabilityRating),
                        Coverage = p.Average(x => x.Coverage),
                        CognitiveComplexity = p.Average(x => x.CognitiveComplexity),
                        Violations = p.Sum(x => x.Violations),
                        SecurityRating = p.Average(x => x.SecurityRating),
                        DuplicatedLines = p.Sum(x => x.DuplicatedLines),
                        LinesOfCode = p.Sum(x => x.LinesOfCode),
                        DuplicatedLinesDensity = p.Average(x => x.DuplicatedLinesDensity),
                        Bugs = p.Sum(x => x.Bugs),
                        TechnicalDebt = p.Average(x => x.TechnicalDebt),
                        ReliabilityRating = p.Average(x => x.ReliabilityRating),
                        Complexity = p.Average(x => x.Complexity),
                        SecurityHotspots = p.Sum(x => x.SecurityHotspots),
                        Tests = p.Sum(x => x.Tests),
                        OverallRating = p.Average(x => x.OverallRating),
                        
                    };
                }
            }
        }
    }
}