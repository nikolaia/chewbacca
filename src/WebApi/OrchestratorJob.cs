using System.Security.Claims;

using Cronos;

using CronScheduler.Extensions.Scheduler;

using Microsoft.Extensions.Options;

using Orchestrator.Service;

namespace WebApi;

public class OrchestratorJob  : IScheduledJob
{
    private readonly IServiceProvider _provider;
    private readonly SchedulerOptions _options;
    private readonly ILogger<OrchestratorJob> _logger;

    public OrchestratorJob(
        IServiceProvider provider,
        SchedulerOptions options)
    {
        _provider = provider;
        _options = options;
        _logger = provider.GetRequiredService<ILogger<OrchestratorJob>>();
    }

    public string Name => nameof(OrchestratorJob);

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var expression = CronExpression.Parse(_options.CronSchedule);
        DateTime? next = expression.GetNextOccurrence(DateTime.UtcNow);
        _logger.LogInformation("Running {JobName} with options {CronSchedule}. Next run is {NextRun}", nameof(OrchestratorJob), _options.CronSchedule, next);
        using var scope = _provider.CreateScope();
        var orchestratorService = scope.ServiceProvider.GetRequiredService<OrchestratorService>();
        await orchestratorService.FetchMapAndSaveEmployeeData();
    }
}