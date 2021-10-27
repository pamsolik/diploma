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
using Newtonsoft.Json;

namespace Cars.Services.Implementations
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly ILogger<AnalysisService> _logger;
        private Timer _timer;
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new();

        private TimeSpan Interval { get; } = TimeSpan.FromSeconds(30);
        private TimeSpan FirstRunAfter { get; } = TimeSpan.FromSeconds(30);

        public const string SonarLoc = "D:/SonarScan";

        private const string MetricsUri =
            "http://localhost:9000/api/measures/component?metricKeys=bugs,code_smells,duplicated_lines,duplicated_lines_density,complexity,cognitive_complexity,violations,coverage,lines,sqale_rating,reliability_rating,security_hotspots,security_rating&component=test";

        public AnalysisService(IServiceScopeFactory scopeFactory, ILogger<AnalysisService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            StartAsync(_stoppingCts.Token);
        }

        ~AnalysisService()
        {
            _stoppingCts.Cancel();
            _timer?.Dispose();
        }

        private void StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecuteTask, null, FirstRunAfter, TimeSpan.FromMilliseconds(-1));
        }

        private void ExecuteTask(object state)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _executingTask = ExecuteTaskAsync(_stoppingCts.Token);
        }

        private async Task ExecuteTaskAsync(CancellationToken stoppingToken)
        {
            try
            {
                // Task.Factory.StartNew(() =>
                //     {
                //         //executes in thread pool.
                //         return GetSomething(); // returns a Task.
                //     }) // returns a Task<Task>.
                //     .Unwrap() // "unwraps" the outer task, returning a proxy
                //     // for the inner one returned by GetSomething().
                //     .ContinueWith(task =>
                //     {
                //         // executes in UI thread.
                //         Prop = task.Result;
                //     }, TaskScheduler.FromCurrentSynchronizationContext());
                
                await RunJobAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "BackgroundTask Failed");
            }

            _timer.Change(Interval, TimeSpan.FromMilliseconds(-1));
        }

        private static async Task PerformScan(string projectDir)
        {
            var cmd =
                $"sonar-scanner.bat -D\"sonar.projectKey=test\" -D\"sonar.sources=.\" " +
                $"-D\"sonar.host.url=http://localhost:9000\" " +
                $"-D\"sonar.login=2a717a00e2600f862f49a8fcc9b28f2029040369\"";
            
            await CommandExecutor.ExecuteCommandAsync(cmd, projectDir);
        }
        
        private async Task RunJobAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var notExamined = context.Applications
                .Where(a => a.CodeQualityAssessment == null || !a.CodeQualityAssessment.Success)
                .ToList();

            if (notExamined.Any())
            {
                var http = new HttpRequestHandler<CodeAnalysis>();
                
                foreach (var ne in notExamined)
                {
                    var projects = context.Projects.Where(p => p.ApplicationId == ne.Id);
                    foreach (var project in projects)
                    {
                        try
                        {
                            var projectDir = Path.Combine(SonarLoc, ne.ApplicantId, project.Id.ToString());
                            var dirInfo = new DirectoryInfo(projectDir);
                            
                            if (dirInfo.Exists)
                            {
                                UpdateFileAttributes(new DirectoryInfo(projectDir));
                                dirInfo.Delete(true);
                            }
                            dirInfo.Create();
                            
                            await RepositoryLoader.Clone("https://github.com/pamsolik/TestRepo", projectDir);

                            await PerformScan(projectDir);
                            
                            var analysis = http.GetResponse(MetricsUri);
                            var ass = new CodeQualityAssessment
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
                                Complexity = analysis.GetValue("complexity"),  //Check
                                SecurityHotspots = analysis.GetValue("security_hotspots"),
                                OverallRating = 0f //TODO: Calculate
                            };
                            ne.CodeQualityAssessment = ass;
                            
                            await context.SaveChangesAsync();
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, "CodeQualityAssessments Error");
                        }
                    }
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }
        
        private static void UpdateFileAttributes(DirectoryInfo dInfo)
        {
            // Set Directory attribute
            dInfo.Attributes &= ~FileAttributes.ReadOnly;

            // get list of all files in the directory and clear 
            // the Read-Only flag

            foreach (var file in dInfo.GetFiles())
            {
                file.Attributes &= ~FileAttributes.ReadOnly;
            }

            // recurse all of the subdirectories
            foreach (var subDir in dInfo.GetDirectories())
            {
                UpdateFileAttributes(subDir);
            }
        }
    }
}