using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Services.Interfaces;

namespace Cars.Services.Implementations
{
    public class AnalysisDataDataService : IAnalysisDataService
    {
        private readonly ApplicationDbContext _context;

        public AnalysisDataDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project> SaveCodeQualityAnalysis(Project project, CodeQualityAssessment ass)
        {
            project.CodeQualityAssessment = ass;
            await _context.SaveChangesAsync();
            return project;
        }

        public List<RecruitmentApplication> GetNotExamined()
        {
            var notFull =  _context.Applications
                .Where(a => a.CodeOverallQualityId == null || a.CodeOverallQuality.Success);
            return notFull.Where(n =>
                    n.Projects.Any(p => p.CodeQualityAssessmentId == null || p.CodeQualityAssessment.Success == false))
                .ToList();
        }

        public List<Project> GetProjects(RecruitmentApplication notExamined)
        {
            return _context.Projects.Where(p =>
                    p.ApplicationId == notExamined.Id &&
                    (p.CodeQualityAssessmentId == null || p.CodeQualityAssessment.Success))
                .ToList();
        }
    }
}