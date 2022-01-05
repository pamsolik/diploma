using Core.DataModels;
using Core.SonarQubeDataModels;
using Services.Interfaces;
using static Services.Other.OverallQualityCalculator;

namespace Services;

public static class CodeQualityAssessmentFactory
{
    public static CodeQualityAssessment? LoadMeasures(this CodeQualityAssessment? coq, CodeAnalysis? analysis,
        bool success)
    {
        if (coq == null) return null;
        coq.Success = success;
        if (analysis is null) return coq;
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

    private static CodeQualityAssessment? CreateInstance(IDateTimeProvider dateTimeProvider, bool success)
    {
        return new CodeQualityAssessment
        {
            CompletedTime = dateTimeProvider.GetTimeNow(),
            Success = success
        };
    }

    public static CodeQualityAssessment? CreateInstance(IDateTimeProvider dateTimeProvider, bool success,
        CodeAnalysis? analysis)
    {
        var res = CreateInstance(dateTimeProvider, success);
        if (analysis is not null) res.LoadMeasures(analysis, success);
        return res;
    }
}