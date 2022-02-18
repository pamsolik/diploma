using Core.DataModels;

namespace Services.Managers.Interfaces;

public interface IAnalysisManager
{
    Task<Project> SaveCodeQualityAnalysis(Project project, CodeQualityAssessment? ass);

    List<RecruitmentApplication> GetNotExaminedApplications();

    List<Project> GetNotExaminedProjects();

    List<Project> GetNotExaminedProjects(RecruitmentApplication notExamined);

    List<Project> GetAllProjects(RecruitmentApplication notExamined);

    Task<RecruitmentApplication> SaveCodeOverallQuality(RecruitmentApplication notExamined, CodeOverallQuality? ass);
}