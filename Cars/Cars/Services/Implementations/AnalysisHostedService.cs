using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cars.Managers.Interfaces;
using Cars.Models.DataModels;
using Cars.Models.Enums;
using Cars.Models.SonarQubeDataModels;
using Cars.Services.Interfaces;
using Cars.Services.Other;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestSharp;
using static Cars.Data.CodeQualityAssessmentFactory;
using static Cars.Data.CodeOverallQualityFactory;
using static Cars.Services.Other.FileService;
using static Cars.Services.Other.OverallQualityCalculator;

namespace Cars.Services.Implementations
{
    public class AnalysisHostedService : CronJobService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<AnalysisHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly SonarQubeRequestHandler _sonarQubeRequestHandler;

        public AnalysisHostedService(IScheduleConfig<AnalysisHostedService> config, IServiceScopeFactory scopeFactory,
            ILogger<AnalysisHostedService> logger, IDateTimeProvider dateTimeProvider,
            SonarQubeRequestHandler sonarQubeRequestHandler)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _sonarQubeRequestHandler = sonarQubeRequestHandler;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("AnalysisHostedService starts");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("AnalysisHostedService is stopping");
            return base.StopAsync(cancellationToken);
        }

        protected override Task DoWork(CancellationToken cancellationToken)
        {
            
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} AnalysisHostedService is working");
            var t = Task.Run(async () => { await PerformFullAnalysis(); }, cancellationToken);
            return t;
        }

        private async Task PerformFullAnalysis()
        {
            using var scope = _scopeFactory.CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<IAnalysisManager>();
            if (manager == null)
            {
                _logger.LogWarning("No AnalysisManager found");
                return;
            }

            var notExamined = manager.GetNotExaminedApplications();

            if (!notExamined.Any())
            {
                _logger.LogInformation("No projects to scan found");
                return;
            }

            var projects = _sonarQubeRequestHandler.GetResponse<Projects>(_sonarQubeRequestHandler.GetProjectsUri());

            if (projects is not null)
            {
                foreach (var application in notExamined)
                {
                    _logger.LogInformation($"Scanning application {application.Id}");
                    var projectsToExamine = manager.GetNotExaminedProjects(application);

                    foreach (var project in projectsToExamine)
                        await ExamineSingleProject(application, project, manager, projects);

                    projectsToExamine = manager.GetNotExaminedProjects(application);
                    if (!projectsToExamine.Any() && application.CodeOverallQualityId is null)
                        await CalculateAndSaveCodeOverallQuality(manager, application);
                }
            }
            else
            {
                _logger.LogWarning($"Cannot reach SonarQube on {_sonarQubeRequestHandler.BasePath}");
            }
        }

        private async Task CalculateAndSaveCodeOverallQuality(IAnalysisManager manager,
            RecruitmentApplication application)
        {
            try
            {
                _logger.LogInformation($"CalculateAndSaveCodeOverallQuality: {application.Id}");
                var projects = manager.GetAllProjects(application);
                var coq = GetCodeOverallQuality(projects, _dateTimeProvider);
                if (coq is null)
                {
                    _logger.LogWarning($"GetCodeOverallQuality for application: {application.Id} is null");
                    return;
                }

                coq.OverallRating = CalculateOverallRating(coq);
                await manager.SaveCodeOverallQuality(application, coq);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CodeQualityAssessments Error");
            }
        }

        private async Task ExamineSingleProject(RecruitmentApplication application, Project project,
            IAnalysisManager manager, Projects projects)
        {
            try
            {
                _logger.LogInformation($"ExamineSingleProject: {project.Id}");
                var projectDir = Path.Combine(SonarQubeRequestHandler.SonarLoc, application.ApplicantId,
                    project.Id.ToString());
                var dirInfo = new DirectoryInfo(projectDir);
                var saved = false;
                var projectKey = $"Project_{project.Id}";

                var projectCreated = projects.Components.Any(c => c.Key == projectKey);

                if (!projectCreated)
                {
                    _logger.LogInformation($"Creating project: {projectKey}");
                    var sonarProject =
                        _sonarQubeRequestHandler.GetResponse<ProjectCreate>(
                            _sonarQubeRequestHandler.GetCreateProjectUri(projectKey), Method.POST);
                    projectCreated = sonarProject is not null;
                }
                else
                {
                    _logger.LogInformation($"TryToReadAnalysis: {projectKey}");
                    saved = await TryToReadAnalysis(project, manager, projectKey);
                }

                if (projectCreated && !saved)
                {
                    _logger.LogInformation($"Project {project.Id} performing scan");
                    EnsureDirectoryIsCreated(dirInfo);

                    RepositoryLoader.Clone(project.Url, projectDir);

                    var solutionsCnt = await PerformScan(projectDir, projectKey, project.Technology);

                    project.SolutionsCnt = solutionsCnt;
                    await TryToReadAnalysis(project, manager, projectKey);
                    //DeleteWithoutPermissions(dirInfo);
                }
                else
                {
                    if (!projectCreated) _logger.LogWarning($"Project {project.Id} failed to create");
                    if (!saved) _logger.LogWarning($"Project {project.Id} not saved");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ExamineSingleProject Error");
            }
        }

        private async Task<bool> TryToReadAnalysis(Project project, IAnalysisManager manager, string projectKey)
        {
            var analysis =
                _sonarQubeRequestHandler.GetResponse<CodeAnalysis>(_sonarQubeRequestHandler.GetMetricsUri(projectKey));

            var loaded = analysis is not null && analysis.Component.Measures.Any();


            var ass = project.CodeQualityAssessmentId is null
                ? CreateInstance(_dateTimeProvider, loaded)
                : CreateInstance(_dateTimeProvider, loaded, analysis);

            _logger.LogInformation($"TryToReadAnalysis: {loaded}");

            if (!loaded)
            {
                if (project.Retries - 3 > project.SolutionsCnt) ass.Success = true;
                project.Retries++;
            }

            await manager.SaveCodeQualityAnalysis(project, ass);
            //GetResponse<string>(GetDeleteProjectUri(projectKey), Method.POST); //TODO: Uncomment
            return true;
        }

        private async Task<int> PerformScan(string projectDir, string projectKey, Technology technology)
        {
            var (dir, projects) = technology switch
            {
                Technology.Other => (projectDir, 0),
                Technology.DotNet => FindAllFiles(projectDir, "*.sln"),
                Technology.Gradle => FindAllFiles(projectDir, "build.gradle"),
                Technology.Maven => FindAllFiles(projectDir, "pom.xml"),
                _ => throw new ArgumentException("This technology isn't supported")
            };

            var cmd = technology switch
            {
                Technology.Other => _sonarQubeRequestHandler.GetNormalScanCommand(projectKey),
                Technology.DotNet => _sonarQubeRequestHandler.GetDotNetScanCommand(projectKey),
                Technology.Gradle => _sonarQubeRequestHandler.GetGradleScanCommand(projectKey),
                Technology.Maven => _sonarQubeRequestHandler.GetMvnScanCommand(projectKey),
                _ => throw new ArgumentException("This technology isn't supported")
            };

            await WriteCommand(Path.Combine(dir, "sonarcmd.txt"), cmd);
            await CommandExecutor.ExecuteCommandAsync(cmd, dir, _logger);
            return projects;
        }

        private static async Task WriteCommand(string loc, string txt)
        {
            await File.WriteAllTextAsync(loc, txt);
        }
    }
}