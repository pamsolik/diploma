using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Models.SonarQubeDataModels;
using Cars.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cars.Services.Implementations
{
    public class ScannerService : IScannerService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly ILogger<ScannerService> _logger;
        private Timer _timer;
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new();

        private TimeSpan Interval { get; } = TimeSpan.FromSeconds(300);
        private TimeSpan FirstRunAfter { get; } = TimeSpan.FromSeconds(30);

        private const string MetricsUri =
            "http://localhost:9000/api/measures/component?metricKeys=bugs,code_smells,duplicated_lines,duplicated_lines_density,complexity,cognitive_complexity,violations,coverage,lines,sqale_rating,reliability_rating,security_hotspots,security_rating&component=aas";

        public ScannerService(IServiceScopeFactory scopeFactory, ILogger<ScannerService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            StartAsync(_stoppingCts.Token);
        }

        ~ScannerService()
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
                await RunJobAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "BackgroundTask Failed");
            }

            _timer.Change(Interval, TimeSpan.FromMilliseconds(-1));
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
                    try
                    {
                        var analysis = http.GetResponse(MetricsUri);
                        var ass = context.CodeQualityAssessments.Add(new CodeQualityAssessment
                        {
                            CompletedTime = DateTime.Now,
                            Success = true
                        });
                        ne.CodeQualityAssessment = ass.Entity;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "CodeQualityAssessments Error");
                    }
                }

                await context.SaveChangesAsync();
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
    }
}