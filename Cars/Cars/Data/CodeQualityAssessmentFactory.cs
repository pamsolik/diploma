using System;
using Cars.Models.DataModels;
using Cars.Models.SonarQubeDataModels;

namespace Cars.Data
{
    public static class CodeQualityAssessmentFactory
    {
        public static CodeQualityAssessment CreateInstance(CodeAnalysis analysis)
        {
            return new()
            {
                CompletedTime = DateTime.Now,
                Success = true,
                CodeSmells = analysis.GetValue("code_smells"),
                Maintainability = analysis.GetValue("sqale_rating"),
                Coverage = analysis.GetValue("coverage"), //Check
                CognitiveComplexity = analysis.GetValue("cognitive_complexity"), //Check
                Violations = analysis.GetValue("violations"),
                SecurityRating = analysis.GetValue("security_rating"),
                DuplicatedLines = analysis.GetValue("duplicated_lines"),
                Lines = analysis.GetValue("lines"),
                DuplicatedLinesDensity = analysis.GetValue("duplicated_lines_density"),
                Bugs = analysis.GetValue("bugs"),
                SqaleRating = analysis.GetValue("security_rating"),
                ReliabilityRating = analysis.GetValue("reliability_rating"),
                Complexity = analysis.GetValue("complexity"), //Check
                SecurityHotspots = analysis.GetValue("security_hotspots"),
                OverallRating = 0f //TODO: Calculate
            };
        }

        public static CodeQualityAssessment CreateInstance()
        {
            return new()
            {
                CompletedTime = DateTime.Now,
                Success = false
            };
        }
    }
}