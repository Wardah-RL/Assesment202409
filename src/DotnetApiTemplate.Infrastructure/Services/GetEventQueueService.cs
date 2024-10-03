using Azure.Core;
using Azure.Storage.Queues;
using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Infrastructure.Services.Request;
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
      string queueName = _queueConfiguration.NameEvent;

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

            var getEventMessage = JsonConvert.DeserializeObject<EventMessageRequest>(getMessage.Message);

            using (var scope = _serviceProvider.CreateScope())
            {
              var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

              var getEvent = await dbContext.Set<MsEvent>()
                              .Include(e=>e.EventLocation)
                             .Where(e => e.Id == getEventMessage.EventId)
                             .FirstOrDefaultAsync(cancellationToken);

              if (getEvent == null)
              {
                //create event
                var newEvent = new MsEvent
                {
                  Id = getEventMessage.EventId,
                  Name = getEventMessage.Name,
                  StartDate = getEventMessage.StartDate,
                  EndDate = getEventMessage.EndDate,
                  CountTicket = getEventMessage.CountTicket,
                };
                await dbContext.InsertAsync(newEvent, cancellationToken);

                foreach (var item in getEventMessage.Location)
                {
                  var newEventLocation = new MsEventLocation
                  {
                    Id = item.EventLocationId,
                    Location = item.Location,
                    EventId = item.EventId
                  };
                  await dbContext.InsertAsync(newEventLocation, cancellationToken);
                }
              }
              else
              {
                if (getMessage.Scenario=="DeleteEvent")
                {
                  //Delete event
                  dbContext.AttachEntity(getEvent);
                  getEvent.IsDeleted = true;

                  foreach (var Item in getEvent.EventLocation)
                  {
                    dbContext.AttachEntity(Item);
                    Item.IsDeleted = true;
                  }
                }
                else 
                {
                  //Update event
                  dbContext.AttachEntity(getEvent);
                  getEvent.Name = getEventMessage.Name;
                  getEvent.StartDate = getEventMessage.StartDate;
                  getEvent.EndDate = getEventMessage.EndDate;
                  getEvent.CountTicket = getEventMessage.CountTicket;
                  await dbContext.SaveChangesAsync(cancellationToken);

                  foreach (var Item in getEvent.EventLocation)
                  {
                    var getLocationRequest = getEventMessage.Location.Where(e => e.Location == Item.Location).FirstOrDefault();

                    if (getLocationRequest == null)
                    {
                      dbContext.AttachEntity(Item);
                      Item.IsDeleted = true;
                    }
                  }

                  foreach (var item in getEventMessage.Location)
                  {
                    var getLocationDb = getEvent.EventLocation.Where(e => e.Location == item.Location).FirstOrDefault();

                    if (getLocationDb == null)
                    {
                      var newEventLocation = new MsEventLocation
                      {
                        Id = item.EventLocationId,
                        Location = item.Location,
                        EventId = item.EventId
                      };
                      await dbContext.InsertAsync(newEventLocation, cancellationToken);

                    }
                  }
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
