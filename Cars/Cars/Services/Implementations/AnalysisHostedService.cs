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
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static Cars.Data.CodeQualityAssessmentFactory;
using static Cars.Data.CodeOverallQualityFactory;
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

            var http = new HttpRequestHandler<CodeAnalysis>();

            foreach (var application in notExamined)
            {
                var projects = service.GetNotExaminedProjects(application);
                if (!projects.Any() && application.CodeOverallQualityId is null)
                {
                    await CalculateAndSaveCodeOverallQuality(service, application);
                }
                else
                {
                    foreach (var project in projects)
                    {
                        await ExamineSingleProject(application, project, http, service);
                    }
                }
            }
        }

        private async Task CalculateAndSaveCodeOverallQuality(IAnalysisDataService service,
            RecruitmentApplication application)
        {
            try
            {
                var projects = service.GetAllProjects(application);
                var coq = GetCodeOverallQuality(projects);
                await service.SaveCodeOverallQuality(application, coq);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CodeQualityAssessments Error");
            }
        }

        private async Task ExamineSingleProject(RecruitmentApplication application, Project project,
            HttpRequestHandler<CodeAnalysis> http, IAnalysisDataService dataService)
        {
            try
            {
                var retry = project.CodeQualityAssessmentId is not null;
                var projectDir = Path.Combine(SonarLoc, application.ApplicantId, project.Id.ToString());
                var dirInfo = new DirectoryInfo(projectDir);

                EnsureDirectoryIsCreated(dirInfo);

                await RepositoryLoader.Clone(project.Url, projectDir);

                //TODO: Add project on SonarQube

                await PerformScan(projectDir);

                var analysis = http.GetResponse(GetMetricsUri("test"));
                var ass = analysis is null ? CreateInstance() : CreateInstance(analysis);
                if (retry) ass.Success = true;
                await dataService.SaveCodeQualityAnalysis(project, ass);
                DeleteWithoutPermissions(dirInfo);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CodeQualityAssessments Error");
            }
        }

        private async Task PerformScan(string projectDir)
        {
            var cmd =
                $"sonar-scanner.bat -D\"sonar.projectKey=test\" -D\"sonar.sources=.\" -D\"sonar.host.url=http://localhost:9000\" -D\"sonar.login=2a717a00e2600f862f49a8fcc9b28f2029040369\"";

            await CommandExecutor.ExecuteCommandAsync(cmd, projectDir, _logger);
        }
    }
}