using Core.DataModels;

namespace Services.Managers.Interfaces;

public interface IAnalysisManager
{
    public Task<Project> SaveCodeQualityAnalysis(Project project, CodeQualityAssessment ass);

    public List<RecruitmentApplication> GetNotExaminedApplications();

    public List<Project> GetNotExaminedProjects(RecruitmentApplication notExamined);

    public List<Project> GetAllProjects(RecruitmentApplication notExamined);

    public Task<RecruitmentApplication> SaveCodeOverallQuality(RecruitmentApplication notExamined,
        CodeOverallQuality ass);
}