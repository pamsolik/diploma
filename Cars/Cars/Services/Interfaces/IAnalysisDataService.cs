using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Models.DataModels;

namespace Cars.Services.Interfaces
{
    public interface IAnalysisDataService
    {
        public Task<Project> SaveCodeQualityAnalysis(Project project, CodeQualityAssessment ass);

        public List<RecruitmentApplication> GetNotExaminedApplications();

        public List<Project> GetNotExaminedProjects(RecruitmentApplication notExamined);

        public List<Project> GetAllProjects(RecruitmentApplication notExamined);

        public Task<RecruitmentApplication> SaveCodeOverallQuality(RecruitmentApplication notExamined,
            CodeOverallQuality ass);
    }
}