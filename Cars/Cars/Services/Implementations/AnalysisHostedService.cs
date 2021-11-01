using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Models.SonarQubeDataModels;
using Cars.Services.Interfaces;
using Cars.Services.Other;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestSharp;
using static Cars.Data.CodeQualityAssessmentFactory;
using static Cars.Data.CodeOverallQualityFactory;
using static Cars.Data.HttpRequestHandler;
using static Cars.Services.Other.SonarQubeRequestHandler;
using static Cars.Services.Other.FileService;

namespace Cars.Services.Implementations
{
    internal class AnalysisHostedService : CronJobService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AnalysisHostedService> _logger;

        private const string SonarLoc = "D:/SonarScan";

        public AnalysisHostedService(IScheduleConfig<AnalysisHostedService> config, IServiceScopeFactory scopeFactory,
            ILogger<AnalysisHostedService> logger)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJobAnalysis starts");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJobAnalysis is stopping");
            return base.StopAsync(cancellationToken);
        }

        protected override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJobAnalysis is working");
            var t = Task.Run(async () => { await PerformFullAnalysis(); }, cancellationToken);
            return t;
        }

        private async Task PerformFullAnalysis()
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IAnalysisDataService>();
            if (service == null) return;

            var notExamined = service.GetNotExaminedApplications();

            if (!notExamined.Any()) return;

            var http = new HttpRequestHandler();
            var projects = GetResponse<Projects>(GetProjectsUri());

            if (projects is not null)
            {
                foreach (var application in notExamined)
                {
                    var projectsToExamine = service.GetNotExaminedProjects(application);

                    foreach (var project in projectsToExamine)
                    {
                        await ExamineSingleProject(application, project, service, projects);
                    }

                    projectsToExamine = service.GetNotExaminedProjects(application);
                    if (!projectsToExamine.Any() && application.CodeOverallQualityId is null)
                    {
                        await CalculateAndSaveCodeOverallQuality(service, application);
                    }
                }
            }
            else
            {
                _logger.LogWarning($"Cannot reach SonarQube on {BasePath}");
            }
        }

        private async Task CalculateAndSaveCodeOverallQuality(IAnalysisDataService service,
            RecruitmentApplication application)
        {
            try
            {
                var projects = service.GetAllProjects(application);
                var coq = GetCodeOverallQuality(projects);
                if (coq is null) return;
                await service.SaveCodeOverallQuality(application, coq);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CodeQualityAssessments Error");
            }
        }

        private async Task ExamineSingleProject(RecruitmentApplication application, Project project, IAnalysisDataService dataService, Projects projects)
        {
            try
            {
                var projectDir = Path.Combine(SonarLoc, application.ApplicantId, project.Id.ToString());
                var dirInfo = new DirectoryInfo(projectDir);
                var saved = false;
                var projectKey = $"Project_{project.Id}";

                var projectCreated = projects.Components.Any(c => c.Key == projectKey);
                
                if (!projectCreated)
                {
                    var sonarProject = GetResponse<ProjectCreate>(GetCreateProjectUri(projectKey), Method.POST);
                    projectCreated = sonarProject is not null;
                }
                else
                {
                    saved = await TryToReadAnalysis(project, dataService, projectKey);
                }
                
                if (projectCreated && !saved)
                {
                    EnsureDirectoryIsCreated(dirInfo);

                    RepositoryLoader.Clone(project.Url, projectDir);
                    
                    await PerformScan(projectDir, projectKey);

                    await TryToReadAnalysis(project, dataService, projectKey);
                    DeleteWithoutPermissions(dirInfo);
                }
                else
                {
                    if (projectCreated) _logger.LogWarning($"Project {project.Id} failed to create");
                    if (!saved) _logger.LogWarning($"Project {project.Id} not saved");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CodeQualityAssessments Error");
            }
        }

        private static async Task<bool> TryToReadAnalysis(Project project, IAnalysisDataService dataService, string projectKey)
        {
            var retry = project.CodeQualityAssessmentId is not null;
            var analysis = GetResponse<CodeAnalysis>(GetMetricsUri(projectKey));

            var loaded = analysis is not null && analysis.Component.Measures.Any();
            
            var ass = loaded? CreateInstance(analysis) : CreateInstance();

            if (!loaded) return false;
            
            if (retry) ass.Success = true;
            await dataService.SaveCodeQualityAnalysis(project, ass);
            GetResponse<string>(GetDeleteProjectUri(projectKey), Method.POST);
            return true;
        }

        private async Task PerformScan(string projectDir, string projectKey)
        {
            var cmd =
                $"sonar-scanner.bat -D\"sonar.projectKey={projectKey}\" -D\"sonar.sources=.\" -D\"sonar.host.url=http://localhost:9000\" -D\"sonar.login={Key}\"";

            await CommandExecutor.ExecuteCommandAsync(cmd, projectDir, _logger);
        }
    }
}