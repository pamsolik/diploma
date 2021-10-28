using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Models.SonarQubeDataModels;
using Cars.Services.Interfaces;
using Cars.Services.Other;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cars.Services.Implementations
{
    public class AnalysisService : CronJobService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly CancellationTokenSource _stoppingCts = new();

        private TimeSpan Interval { get; } = TimeSpan.FromSeconds(30);
        private TimeSpan FirstRunAfter { get; } = TimeSpan.FromSeconds(30);

        private const string SonarLoc = "D:/SonarScan";

        private const string MetricsUri =
            "http://localhost:9000/api/measures/component?metricKeys=bugs,code_smells,duplicated_lines,duplicated_lines_density,complexity,cognitive_complexity,violations,coverage,lines,sqale_rating,reliability_rating,security_hotspots,security_rating&component=test";


        private readonly ILogger<AnalysisService> _logger;

        public AnalysisService(IScheduleConfig<AnalysisService> config, ILogger<AnalysisService> logger,
            IServiceScopeFactory scopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJobAnalysis starts");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJobAnalysis is working");
            var t = PerformFullAnalysisTask(cancellationToken);
            return t;
        }

        private Task PerformFullAnalysisTask(CancellationToken cancellationToken)
        {
            return Task.Run(async () => { await PerformFullAnalysis(cancellationToken); }, cancellationToken);
        }

        private async Task PerformFullAnalysis(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var notExamined = GetNotExamined(context);

            if (!notExamined.Any()) return;

            var http = new HttpRequestHandler<CodeAnalysis>();

            foreach (var application in notExamined)
            {
                var projects = GetProjects(context, application);
                foreach (var project in projects)
                {
                    await ExamineSingleProject(application, project, http, context, cancellationToken);
                }
            }
        }

        private async Task ExamineSingleProject(RecruitmentApplication application, Project project,
            HttpRequestHandler<CodeAnalysis> http, DbContext context, CancellationToken cancellationToken)
        {
            try
            {
                var projectDir = Path.Combine(SonarLoc, application.ApplicantId, project.Id.ToString());

                EnsureDirectoryIsCreated(projectDir);

                await RepositoryLoader.Clone(project.Url, projectDir);

                await PerformScan(projectDir);

                var analysis = http.GetResponse(MetricsUri);
                var ass = analysis is null ? GetErrorCodeQualityAssessment() : GetCodeQualityAssessment(analysis);

                project.CodeQualityAssessment = ass;
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CodeQualityAssessments Error");
            }
        }

        private static CodeQualityAssessment GetCodeQualityAssessment(CodeAnalysis analysis)
        {
            return new CodeQualityAssessment
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

        private static CodeQualityAssessment GetErrorCodeQualityAssessment()
        {
            return new CodeQualityAssessment
            {
                CompletedTime = DateTime.Now,
                Success = false
            };
        }

        private static void EnsureDirectoryIsCreated(string projectDir)
        {
            var dirInfo = new DirectoryInfo(projectDir);

            if (dirInfo.Exists)
            {
                UpdateFileAttributes(new DirectoryInfo(projectDir));
                dirInfo.Delete(true);
            }

            dirInfo.Create();
        }

        private static IQueryable<Project> GetProjects(ApplicationDbContext context, RecruitmentApplication notExamined)
        {
            return context.Projects.Where(p =>
                p.ApplicationId == notExamined.Id &&
                (p.CodeQualityAssessmentId == null || p.CodeQualityAssessment.Success));
        }

        private static List<RecruitmentApplication> GetNotExamined(ApplicationDbContext context)
        {
            return context.Applications
                .Where(a => a.Projects.Any(
                    p => p.CodeQualityAssessmentId == null || p.CodeQualityAssessment.Success))
                .ToList();
        }

        private static async Task PerformScan(string projectDir)
        {
            var cmd =
                $"sonar-scanner.bat -D\"sonar.projectKey=test\" -D\"sonar.sources=.\" -D\"sonar.host.url=http://localhost:9000\" -D\"sonar.login=2a717a00e2600f862f49a8fcc9b28f2029040369\"";

            await CommandExecutor.ExecuteCommandAsync(cmd, projectDir);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJobAnalysis is stopping");
            return base.StopAsync(cancellationToken);
        }

        private static void UpdateFileAttributes(DirectoryInfo dInfo)
        {
            dInfo.Attributes &= ~FileAttributes.ReadOnly;

            foreach (var file in dInfo.GetFiles())
            {
                file.Attributes &= ~FileAttributes.ReadOnly;
            }

            foreach (var subDir in dInfo.GetDirectories())
            {
                UpdateFileAttributes(subDir);
            }
        }
    }
}