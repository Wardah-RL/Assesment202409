using Azure.Core;
using Azure.Storage.Queues;
using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Helpers;
using DotnetApiTemplate.Shared.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;

namespace DotnetApiTemplate.Infrastructure.Services
{
  public class GetEventQueueService : IGetEventQueue
  {
    private readonly QueueConfiguration _queueConfiguration;
    private readonly CancellationToken cancellationToken;
    private readonly IServiceProvider _serviceProvider;

    public GetEventQueueService(QueueConfiguration queueConfiguration, IServiceProvider serviceProvider)
    {
      _queueConfiguration = queueConfiguration;
      _serviceProvider = serviceProvider;
    }
    public async void GetQueueAsync()
    {
      string connectionString = _queueConfiguration.Connection;
      string queueName = _queueConfiguration.Name;

      QueueClient queue = new QueueClient(connectionString, queueName);

      if (queue.Exists())
      {
        try
        {
          var listReceiveMessages = queue.ReceiveMessages(maxMessages: 32).Value.ToList();

          foreach (var message in listReceiveMessages)
          {
            var jsonString = message.Body.ToString();
            var getMessage = JsonConvert.DeserializeObject<SendQueueRequest>(jsonString);
            if (getMessage == null)
              continue;

            if (getMessage.Message == null)
              continue;

            var getEvent = JsonConvert.DeserializeObject<MsEventBroker>(getMessage.Message);

            using (var scope = _serviceProvider.CreateScope())
            {
              var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

              var getEventBroker = await dbContext.Set<MsEvent>()
                              .Where(e => e.Id == getEvent.Id)
                              .FirstOrDefaultAsync(cancellationToken);

              if (getEventBroker == null)
              {
                //create event
                var newEvent = new MsEvent
                {
                  Id = getEvent.Id,
                  Name = getEvent.Name,
                  StartDate = getEvent.StartDate,
                  EndDate = getEvent.EndDate,
                  Lokasi = getEvent.Lokasi,
                  JumlahTiket = getEvent.JumlahTiket,
                };

                await dbContext.InsertAsync(newEvent, cancellationToken);
              }
              else
              {
                //Update event
                bool isUpdate = false;

                if (getEventBroker.Name != getEvent.Name)
                  isUpdate = true;
                if (getEventBroker.StartDate != getEvent.StartDate)
                  isUpdate = true;
                if (getEventBroker.EndDate != getEvent.EndDate)
                  isUpdate = true;
                if (getEventBroker.Lokasi != getEvent.Lokasi)
                  isUpdate = true;
                if (getEventBroker.JumlahTiket != getEvent.JumlahTiket)
                  isUpdate = true;

                if (isUpdate)
                {
                  dbContext.AttachEntity(getEventBroker);
                  getEventBroker.Name = getEvent.Name;
                  getEventBroker.StartDate = getEvent.StartDate;
                  getEventBroker.EndDate = getEvent.EndDate;
                  getEventBroker.Lokasi = getEvent.Lokasi;
                  getEventBroker.JumlahTiket = getEvent.JumlahTiket;
                }
              }

              await dbContext.SaveChangesAsync(cancellationToken);
            }

            //remove queue
            queue.DeleteMessage(message.MessageId, message.PopReceipt);
          }
        }
        catch (Exception ex) 
        { 
        
        }
        
      }
    }
  }
}
