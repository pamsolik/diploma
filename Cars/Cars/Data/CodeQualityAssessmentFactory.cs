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
                //Complexity
                Complexity = analysis.GetValue("complexity"),
                CognitiveComplexity = analysis.GetValue("cognitive_complexity"),
                //Duplications
                DuplicatedLines = analysis.GetValue("duplicated_lines"),
                DuplicatedLinesDensity = analysis.GetValue("duplicated_lines_density"),
                //Issues
                Violations = analysis.GetValue("violations"),
                //Maintability
                CodeSmells = analysis.GetValue("code_smells"),
                MaintainabilityRating = analysis.GetValue("sqale_rating"),
                TechnicalDebt = analysis.GetValue("sqale_index"),
                //Reliability
                Bugs = analysis.GetValue("bugs"),
                ReliabilityRating = analysis.GetValue("reliability_rating"),
                //Tests
                Coverage = analysis.GetValue("coverage"),
                Tests = analysis.GetValue("tests"),
                //Security
                SecurityRating = analysis.GetValue("security_rating"),
                SecurityHotspots = analysis.GetValue("security_hotspots"),
                //Size
                LinesOfCode = analysis.GetValue("lines"),
                //Overall
                OverallRating = null
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