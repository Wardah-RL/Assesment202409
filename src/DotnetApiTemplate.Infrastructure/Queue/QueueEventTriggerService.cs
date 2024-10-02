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
  public class QueueEventTriggerService : BackgroundService
  {
    private readonly ILogger<QueueEventTriggerService> _logger;
    private readonly IGetEventQueue _getQueue;
    public QueueEventTriggerService(ILogger<QueueEventTriggerService> logger, IGetEventQueue getQueue)
    {
      _logger = logger;
      _getQueue = getQueue;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        _getQueue.GetQueueAsync();
        await Task.Delay(1000, stoppingToken);
      }
    }
  }

}
