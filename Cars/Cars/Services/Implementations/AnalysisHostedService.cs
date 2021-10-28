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
using static Cars.Services.Other.CodeQualityAssessmentFactory;
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

            var notExamined = service.GetNotExamined();

            if (!notExamined.Any()) return;

            var http = new HttpRequestHandler<CodeAnalysis>();

            foreach (var application in notExamined)
            {
                var projects = service.GetProjects(application);
                foreach (var project in projects)
                {
                    await ExamineSingleProject(application, project, http, service);
                }
            }
        }

        private async Task ExamineSingleProject(RecruitmentApplication application, Project project,
            HttpRequestHandler<CodeAnalysis> http, IAnalysisDataService dataService)
        {
            try
            {
                var projectDir = Path.Combine(SonarLoc, application.ApplicantId, project.Id.ToString());
                var dirInfo = new DirectoryInfo(projectDir);
                
                EnsureDirectoryIsCreated(dirInfo);

                await RepositoryLoader.Clone(project.Url, projectDir);

                //TODO: Add project on SonarQube

                await PerformScan(projectDir);

                var analysis = http.GetResponse(GetMetricsUri("test"));
                var ass = analysis is null ? CreateInstance() : CreateInstance(analysis);

                await dataService.SaveCodeQualityAnalysis(project, ass);
                DeleteWithoutPermissions(dirInfo);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CodeQualityAssessments Error");
            }
        }
        
        private static async Task PerformScan(string projectDir)
        {
            var cmd =
                $"sonar-scanner.bat -D\"sonar.projectKey=test\" -D\"sonar.sources=.\" -D\"sonar.host.url=http://localhost:9000\" -D\"sonar.login=2a717a00e2600f862f49a8fcc9b28f2029040369\"";

            await CommandExecutor.ExecuteCommandAsync(cmd, projectDir);
        }
    }
}