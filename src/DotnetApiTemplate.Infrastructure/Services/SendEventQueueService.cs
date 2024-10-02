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
  public class SendEventQueueService : ISendEventQueue
  {
    private readonly ConcurrentQueue<IDictionary<string, object>> _queue = new ConcurrentQueue<IDictionary<string, object>>();
    private readonly QueueConfiguration _queueConfiguration;

    public SendEventQueueService(QueueConfiguration queueConfiguration) 
    {
      _queueConfiguration = queueConfiguration;
    }

    public Task SendQueueAsync(SendQueueRequest paramQueue)
    {
      string connectionString = _queueConfiguration.Connection;
      string queueName = _queueConfiguration.Name;

      QueueClient queue = new QueueClient(connectionString, queueName);
      queue.Create();
      string jsonString = JsonConvert.SerializeObject(paramQueue); 
      queue.SendMessage(jsonString);
      return Task.CompletedTask;
    }
  }
}
