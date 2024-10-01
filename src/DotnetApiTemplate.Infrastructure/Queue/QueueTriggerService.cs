using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Infrastructure.Queue
{
  public class QueueTriggerService : BackgroundService
  {
    private readonly ILogger<QueueTriggerService> _logger;
    public QueueTriggerService(ILogger<QueueTriggerService> logger)
    {
      _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        GetQueueService.coba();
        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}
