using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models;
using DotnetApiTemplate.Shared.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Infrastructure.Services
{
  public class GetEventQueueService : IGetEventQueue
  {
    private readonly QueueConfiguration _queueConfiguration;

    public GetEventQueueService(QueueConfiguration queueConfiguration)
    {
      _queueConfiguration = queueConfiguration;
    }
    public void GetQueueAsync()
    {
      string connectionString = _queueConfiguration.Connection;
      string queueName = _queueConfiguration.Name;






    }
  }
}
