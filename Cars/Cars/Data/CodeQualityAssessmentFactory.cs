﻿using Cars.Models.DataModels;
using Cars.Models.SonarQubeDataModels;
using Cars.Services.Interfaces;
using static Cars.Services.Other.OverallQualityCalculator;

namespace Cars.Data
{
    public static class CodeQualityAssessmentFactory
    {
        private static CodeQualityAssessment LoadMeasures(this CodeQualityAssessment coq, CodeAnalysis analysis)
        {
            if (!coq.Success) return coq;
            //Complexity
            coq.Complexity = analysis.GetValue("complexity");
            coq.CognitiveComplexity = analysis.GetValue("cognitive_complexity");
            //Duplications
            coq.DuplicatedLines = analysis.GetValue("duplicated_lines");
            coq.DuplicatedLinesDensity = analysis.GetValue("duplicated_lines_density");
            //Issues
            coq.Violations = analysis.GetValue("violations");
            //Maintability
            coq.CodeSmells = analysis.GetValue("code_smells");
            coq.MaintainabilityRating = analysis.GetValue("sqale_rating");
            coq.TechnicalDebt = analysis.GetValue("sqale_index");
            //Reliability
            coq.Bugs = analysis.GetValue("bugs");
            coq.ReliabilityRating = analysis.GetValue("reliability_rating");
            //Tests
            coq.Coverage = analysis.GetValue("coverage");
            coq.Tests = analysis.GetValue("tests");
            //Security
            coq.SecurityRating = analysis.GetValue("security_rating");
            coq.SecurityHotspots = analysis.GetValue("security_hotspots");
            //Size
            coq.LinesOfCode = analysis.GetValue("lines");
            //Overall
            coq.OverallRating = CalculateOverallRating(coq);
            return coq;
        }

        public static CodeQualityAssessment CreateInstance(IDateTimeProvider dateTimeProvider, bool success)
        {
            return new CodeQualityAssessment
            {
                CompletedTime = dateTimeProvider.GetTimeNow(),
                Success = success
            };
        }

        public static CodeQualityAssessment CreateInstance(IDateTimeProvider dateTimeProvider, bool success,
            CodeAnalysis analysis)
        {
            var res = CreateInstance(dateTimeProvider, success);
            res = res.LoadMeasures(analysis);
            return res;
        }
    }
}