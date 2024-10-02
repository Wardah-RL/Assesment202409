using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using DotnetApiTemplate.Core.Abstractions;
using System.Collections.Concurrent;
using DotnetApiTemplate.Core.Models;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using DotnetApiTemplate.Shared.Abstractions.Models;

namespace DotnetApiTemplate.Infrastructure.Services
{
  public class SendQueueService : ISendQueue
  {
    private readonly ConcurrentQueue<IDictionary<string, object>> _queue = new ConcurrentQueue<IDictionary<string, object>>();
    private readonly QueueConfiguration _queueConfiguration;

    public SendQueueService(QueueConfiguration queueConfiguration) 
    {
      _queueConfiguration = queueConfiguration;
    }

    public void SendQueueAsync(SendQueueRequest paramQueue)
    {
      string connectionString = _queueConfiguration.Connection;
      string queueName = string.Empty;

      if (paramQueue.Scenario == "Event") 
        queueName = _queueConfiguration.Name;

      if(!string.IsNullOrEmpty(queueName) && !string.IsNullOrEmpty(connectionString))
      {
        QueueClient queue = new QueueClient(connectionString, queueName);
        queue.Create();
        string jsonString = JsonConvert.SerializeObject(paramQueue);
        queue.SendMessage(jsonString);
      }
    }
  }
}
