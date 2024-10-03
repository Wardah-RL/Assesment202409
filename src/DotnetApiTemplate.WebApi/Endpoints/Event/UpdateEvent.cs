using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Infrastructure.Services.Request;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Helpers;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using DotnetApiTemplate.WebApi.Endpoints.Event.Validator;
using DotnetApiTemplate.WebApi.Endpoints.UserManagement.Scopes;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace DotnetApiTemplate.WebApi.Endpoints.Event
{
  public class UpdateEvent : BaseEndpointWithoutResponse<UpdateEventRequest>
  {
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<UpdateEventRequest> _localizer;
    private readonly ISendQueue _emailQueue;
    public UpdateEvent(IDbContext dbContext,
        ISendQueue emailQueue,
        IStringLocalizer<UpdateEventRequest> localizer)
    {
      _dbContext = dbContext;
      _emailQueue = emailQueue;
      _localizer = localizer;
    }

    [HttpPut("event")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Update event API",
        Description = "",
        OperationId = "Event.UpdateEvent",
        Tags = new[] { "Event" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(UpdateEventRequest request,
        CancellationToken cancellationToken = new())
    {
      var validator = new UpdateEventValidator();
      var validationResult = await validator.ValidateAsync(request, cancellationToken);
      if (!validationResult.IsValid)
        return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

      var getEventBroker = await _dbContext.Set<MsEventBroker>()
                              .Include(e=>e.EventLocationBroker)
                              .Where(e => e.Id == request.EventId)
                              .FirstOrDefaultAsync(cancellationToken);

      if (getEventBroker == null)
        return BadRequest(Error.Create(string.Format(_localizer["event-not-found"], request.Name)));

      _dbContext.AttachEntity(getEventBroker);
      getEventBroker.Name = request.Name;
      getEventBroker.StartDate = request.StartDate;
      getEventBroker.EndDate = request.EndDate;
      getEventBroker.CountTicket = request.CountTicket;

      foreach(var Item in getEventBroker.EventLocationBroker)
      {
        var getLocationRequest = request.Location.Where(e => e == Item.Location).FirstOrDefault();

        if (getLocationRequest==null)
        {
          _dbContext.AttachEntity(Item);
          Item.IsDeleted = true;
        }
      }

      foreach (var item in request.Location)
      {
        var getLocationDb = getEventBroker.EventLocationBroker.Where(e => e.Location == item).FirstOrDefault();

        if (getLocationDb == null)
        {
          var newEventLocationBroker = new MsEventLocationBroker
          {
            Id = new UuidV7().Value,
            Location = item,
            EventBrokerId = getEventBroker.Id
          };
          await _dbContext.InsertAsync(newEventLocationBroker, cancellationToken);
        }
      }
      await _dbContext.SaveChangesAsync(cancellationToken);

      #region MessageBroker
      var getEventBrokerMessage = await _dbContext.Set<MsEventBroker>()
                                  .Include(e => e.EventLocationBroker)
                                  .Where(e => e.Id == getEventBroker.Id)
                                  .Select(e => new EventMessageRequest
                                  {
                                    EventId = e.Id,
                                    CountTicket = e.CountTicket,
                                    EndDate = e.EndDate,
                                    StartDate = e.StartDate,
                                    Location = e.EventLocationBroker.Select(f => new EventLocationRequest
                                    {
                                      EventId = f.EventBrokerId,
                                      EventLocationId = f.Id,
                                      Location = f.Location
                                    }).ToList(),
                                    Name = e.Name

                                  })
                                  .FirstOrDefaultAsync(cancellationToken);

      SendQueueRequest _paramQueue = new SendQueueRequest
      {
        Message = JsonSerializer.Serialize(getEventBrokerMessage),
        Scenario = "UpdateEvent",
        Scope = "Event"
      };

      _emailQueue.SendQueueAsync(_paramQueue);
      #endregion

      return NoContent();
    }
  }
}
