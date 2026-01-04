using System;
using System.Threading;
using FemFitPlus.Data;
using FemFitPlus.Models;
using FemFitPlus.Services;

internal interface IScopedProcessingService
{
    Task DoWork(CancellationToken stoppingToken);
}

internal class ScopedProcessingService(ILogger<ScopedProcessingService> logger, FemFitPlusContext context) : IScopedProcessingService
{
    private int executionCount = 0;
    private readonly ILogger _logger = logger;
    private readonly FemFitPlusContext _context = context;

    public async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            executionCount++;
            var count = _context.Profiles.Count();
            _logger.LogInformation("Scoped Processing Service is working. Count: {Count} Execution Count: {ExecutionCount}", count, executionCount);
            await Task.Delay(10000, stoppingToken);
        }
    }
}

public class ConsumeScopedServiceHostedService(IServiceProvider services, ILogger<ConsumeScopedServiceHostedService> logger) : BackgroundService
{
    private readonly IServiceProvider _services = services;
    private readonly ILogger _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _services.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();
        await scopedService.DoWork(stoppingToken);
    }
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");
        await base.StopAsync(stoppingToken);
    }
}
