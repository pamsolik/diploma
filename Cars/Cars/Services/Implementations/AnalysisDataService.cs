using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cars.Services.Implementations
{
    public class AnalysisDataService : IAnalysisDataService
    {
        private readonly ApplicationDbContext _context;

        public AnalysisDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project> SaveCodeQualityAnalysis(Project project, CodeQualityAssessment ass)
        {
            project.CodeQualityAssessment = ass;
            await _context.SaveChangesAsync();
            return project;
        }

        public List<RecruitmentApplication> GetNotExaminedApplications()
        {
            return _context.Applications
                .Where(a => a.CodeOverallQualityId == null || a.CodeOverallQuality.Success)
                .ToList();
        }

        public List<Project> GetNotExaminedProjects(RecruitmentApplication notExamined)
        {
            return _context.Projects.Where(p =>
                    p.ApplicationId == notExamined.Id &&
                    (p.CodeQualityAssessmentId == null || !p.CodeQualityAssessment.Success))
                .ToList();
        }

        public List<Project> GetAllProjects(RecruitmentApplication notExamined)
        {
            var x = _context.Projects.Where(p => p.ApplicationId == notExamined.Id).ToList();
                x.ForEach(p => p.CodeQualityAssessment = 
                        _context.CodeQualityAssessments.First(c => c.Id == p.CodeQualityAssessmentId));
                return x;
        }

        public async Task<RecruitmentApplication> SaveCodeOverallQuality(RecruitmentApplication application,
            CodeOverallQuality ass)
        {
            application.CodeOverallQuality = ass;
            await _context.SaveChangesAsync();
            return application;
        }
    }
}