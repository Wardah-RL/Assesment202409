using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Infrastructure.Services.Request;
using DotnetApiTemplate.Shared.Abstractions.Contexts;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace DotnetApiTemplate.WebApi.Endpoints.Event
{
  public class DeleteEvent : BaseEndpointWithoutResponse<DeleteEventRequest>
  {
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<DeleteEvent> _localizer;
    private readonly ISendQueue _emailQueue;
    public DeleteEvent(IDbContext dbContext,
        ISendQueue emailQueue,
        IStringLocalizer<DeleteEvent> localizer)
    {
      _dbContext = dbContext;
      _emailQueue = emailQueue;
      _localizer = localizer;
    }

    [HttpDelete("event/{eventId}")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Delete event API",
        Description = "",
        OperationId = "Event.DeleteEvent",
        Tags = new[] { "Event" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(
        DeleteEventRequest request,
        CancellationToken cancellationToken = new())
    {
      var geteventBroker = await _dbContext.Set<MsEventBroker>()
                      .Include(e=>e.EventLocationBroker)
                      .Where(e => e.Id == request.EventId)
                      .FirstOrDefaultAsync(cancellationToken);

      if (geteventBroker == null)
        return BadRequest(Error.Create(string.Format(_localizer["event-not-found"], request.EventId)));

      _dbContext.AttachEntity(geteventBroker);
      geteventBroker.IsDeleted = true;

      foreach (var Item in geteventBroker.EventLocationBroker)
      {
          _dbContext.AttachEntity(Item);
          Item.IsDeleted = true;
      }

      await _dbContext.SaveChangesAsync(cancellationToken);

      #region MessageBroker
      EventMessageRequest getEventBrokerMessage = new EventMessageRequest
      {
        EventId = geteventBroker.Id,
        CountTicket = geteventBroker.CountTicket,
        EndDate = geteventBroker.EndDate,
        StartDate = geteventBroker.StartDate,
        Location = geteventBroker.EventLocationBroker.Select(f => new EventLocationRequest
        {
          EventId = f.EventBrokerId,
          EventLocationId = f.Id,
          Location = f.Location
        }).ToList(),
        Name = geteventBroker.Name
      };
      
      SendQueueRequest _paramQueue = new SendQueueRequest
      {
        Message = JsonSerializer.Serialize(getEventBrokerMessage),
        Scenario = "DeleteEvent",
        Scope = "Event"
      };

      _emailQueue.SendQueueAsync(_paramQueue);
      #endregion

      return NoContent();
    }
  }
}
