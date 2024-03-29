﻿using Core.DataModels;
using Core.Enums;
using Core.SonarQubeDataModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Services.Managers.Interfaces;
using Services.Other;
using static Services.Other.FileService;
using static Services.CodeQuality.OverallQualityCalculator;
using static Services.CodeQuality.CodeOverallQualityFactory;
using static Services.CodeQuality.CodeQualityAssessmentFactory;

namespace Services.Implementations;

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
        _logger.LogInformation("{Dt:hh:mm:ss} AnalysisHostedService is working", DateTime.Now);
        var t = Task.Run(async () =>
        {
            try
            {
                await PerformFullAnalysis();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PerformFullAnalysis error");
            }
        }, cancellationToken);
        return t;
    }

    private static IAnalysisManager GetAnalysisManager(IServiceScope scope)
    {
        return scope.ServiceProvider.GetRequiredService<IAnalysisManager>();
    }

    private async Task PerformFullAnalysis()
    {
        using var scope = _scopeFactory.CreateScope();
        var manager = GetAnalysisManager(scope);
        var projects = await _sonarQubeRequestHandler.GetExistingProjects();
        if (projects is null)
        {
            _logger.LogWarning("Cannot reach SonarQube on {Url}", _sonarQubeRequestHandler.BasePath);
            return;
        }

        var notExaminedApplications = manager.GetNotExaminedApplications();
        var notExaminedProjects = manager.GetNotExaminedProjects();
        if (!notExaminedApplications.Any() && !notExaminedProjects.Any())
        {
            _logger.LogInformation("No projects to scan found");
            return;
        }

        var cnt = notExaminedApplications.Count;

        foreach (var application in notExaminedApplications)
        {
            _logger.LogInformation("Scanning application {Id}", application.Id);
            var projectsToExamine = manager.GetNotExaminedProjects(application);

            foreach (var project in projectsToExamine)
                await ExamineSingleProject(application, project, manager, projects);

            projectsToExamine = manager.GetNotExaminedProjects(application);
            if (!projectsToExamine.Any())
            {
                cnt -= 1;
                await CalculateAndSaveCodeOverallQuality(manager, application);
            }
        }

        if (cnt == 0)
            foreach (var project in notExaminedProjects)
                await ExamineSingleProject(null, project, manager, projects);
    }

    private async Task CalculateAndSaveCodeOverallQuality(IAnalysisManager manager,
        RecruitmentApplication application)
    {
        try
        {
            _logger.LogInformation("CalculateAndSaveCodeOverallQuality: {Id}", application.Id);
            var projects = manager.GetAllProjects(application);
            var coq = GetCodeOverallQuality(projects, _dateTimeProvider);
            if (coq is null)
            {
                _logger.LogWarning("GetCodeOverallQuality for application: {Id} is null", application.Id);
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

    private async Task ExamineSingleProject(RecruitmentApplication? application, Project project,
        IAnalysisManager manager, Projects? projects)
    {
        try
        {
            _logger.LogInformation("ExamineSingleProject: {Id}", project.Id);
            var (projectDir, dirInfo, projectKey) = GetProjectConstants(application, project);
            var saved = false;

            var projectCreated = projects?.Components != null && projects.Components.Any(c => c.Key == projectKey);

            if (!projectCreated)
            {
                _logger.LogInformation("Creating project: {ProjectKey}", projectKey);
                var sonarProject = await _sonarQubeRequestHandler.CreateProject(projectKey);
                projectCreated = sonarProject is not null;
            }
            else
            {
                _logger.LogInformation("TryToReadAnalysis: {ProjectKey}", projectKey);
                saved = await TryToReadAnalysis(project, manager, projectKey);
            }

            if (projectCreated && !saved)
            {
                _logger.LogInformation("Project {Id} performing scan", project.Id);
                EnsureDirectoryIsCreated(dirInfo);

                RepositoryLoader.Clone(project.Url, projectDir);

                var solutionsCnt = await PerformScan(projectDir, projectKey, project.Technology);

                project.SolutionsCnt = solutionsCnt;
                //await TryToReadAnalysis(project, manager, projectKey);
                DeleteWithoutPermissions(dirInfo);
            }
            else
            {
                if (!projectCreated) _logger.LogWarning("Project {Id} failed to create", project.Id);
                if (!saved) _logger.LogWarning("Project {Id} not saved", project.Id);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "ExamineSingleProject Error");
        }
    }

    private (string, DirectoryInfo, string) GetProjectConstants(RecruitmentApplication? application,
        Project project)
    {
        var projectDir = Path.Combine(_sonarQubeRequestHandler.SonarLoc, "scans", application is null ? "Projects" : application.ApplicantId ?? "XXX",
            project.Id.ToString());
        var directoryInfo = new DirectoryInfo(projectDir);
        var projectKey = $"Project_{project.Id}";
        
        return (projectDir, directoryInfo, projectKey);
    }

    private async Task<bool> TryToReadAnalysis(Project project, IAnalysisManager manager, string projectKey)
    {
        var analysis = await _sonarQubeRequestHandler.GetCodeAnalysis(projectKey);

        var loaded = analysis?.Component?.Measures != null && analysis.Component != null && analysis.Component.Measures.Any();

        var ass = project.CodeQualityAssessment is null
            ? CreateInstance(_dateTimeProvider, loaded, analysis)
            : project.CodeQualityAssessment.LoadMeasures(analysis, loaded);

        _logger.LogInformation("TryToReadAnalysis: {Loaded}", loaded);

        if (!loaded)
        {
            if (project.Retries - 5 > project.SolutionsCnt)
                if (ass != null)
                    ass.Success = true;
            project.Retries++;
        }

        await manager.SaveCodeQualityAnalysis(project, ass);
        await _sonarQubeRequestHandler.DeleteProject(projectKey);
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
            Technology.DotNetFramework => FindAllFiles(projectDir, "*.sln"),
            _ => throw new ArgumentException("This technology isn't supported")
        };

        var cmd = technology switch
        {
            Technology.Other => _sonarQubeRequestHandler.GetNormalScanCommand(projectKey),
            Technology.DotNet => _sonarQubeRequestHandler.GetDotNetScanCommand(projectKey),
            Technology.Gradle => _sonarQubeRequestHandler.GetGradleScanCommand(projectKey),
            Technology.Maven => _sonarQubeRequestHandler.GetMvnScanCommand(projectKey),
            Technology.DotNetFramework => _sonarQubeRequestHandler.GetDotNetFrameworkCommand(projectKey),
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