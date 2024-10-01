using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using DotnetApiTemplate.Core.Abstractions;
using System.Collections.Concurrent;
using DotnetApiTemplate.Core.Models;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace DotnetApiTemplate.Infrastructure.Services
{
  public class SendQueueService : ISendQueue
  {
    private readonly ConcurrentQueue<IDictionary<string, object>> _queue = new ConcurrentQueue<IDictionary<string, object>>();

    public Task SendQueueAsync(SendQueueRequest paramQueue)
    {
      string connectionString = "DefaultEndpointsProtocol=https;AccountName=bssattendancestorage;AccountKey=FyAA9fZLmt4Uyc4WNWg5mX5f8rZJl9DTsRPJaGiD3ZdmRRt4+Zip1fQA3q5A7/mEAvqi/DyZeUy1BZgVsV9hBg==;EndpointSuffix=core.windows.net";
      string queueName = "coba";

      QueueClient queue = new QueueClient(connectionString, queueName);
      queue.Create();
      string jsonString = JsonConvert.SerializeObject(paramQueue); 
      queue.SendMessage(jsonString);

      //var listReceiveMessages = queue.ReceiveMessages(10).Value;
      //foreach (QueueMessage messages in listReceiveMessages)
      //{
      //  //Write your code here to process the messages
      //  //queue.DeleteMessage(messages.MessageId, messages.PopReceipt);
      //  //Delete the message once it has been processed
      //}

      return Task.CompletedTask;
    }
  }
}
