using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.DataModels;

namespace Cars.Services.Interfaces
{
    public interface IAnalysisDataService
    {

        public Task<Project> SaveCodeQualityAnalysis(Project project, CodeQualityAssessment ass);

        public List<RecruitmentApplication> GetNotExamined();

        public List<Project> GetProjects(RecruitmentApplication notExamined);
    }
}