using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Infrastructure.Services.Request;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using DotnetApiTemplate.Shared.Abstractions.Helpers;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using DotnetApiTemplate.WebApi.Endpoints.Event.Validator;
using DotnetApiTemplate.WebApi.Endpoints.UserManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.UserManagement.Scopes;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Text.Json;

namespace DotnetApiTemplate.WebApi.Endpoints.Event
{
  public class CreateEvent : BaseEndpointWithoutResponse<CreateEventRequest>
  {
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<CreateEvent> _localizer;
    private readonly ISendQueue _emailQueue;
    public CreateEvent(IDbContext dbContext,
        ISendQueue emailQueue,
        IStringLocalizer<CreateEvent> localizer)
    {
      _dbContext = dbContext;
      _emailQueue = emailQueue;
      _localizer = localizer;
    }

    [HttpPost("event")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Create event API",
        Description = "",
        OperationId = "Event.CreateEvent",
        Tags = new[] { "Event" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateEventRequest request,
        CancellationToken cancellationToken = new())
    {
      var validator = new CreateEventValidator();
      var validationResult = await validator.ValidateAsync(request, cancellationToken);
      if (!validationResult.IsValid)
        return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

      var newEventBroker = new MsEventBroker
      {
        Id = new UuidV7().Value,
        Name = request.Name,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
        CountTicket = request.CountTicket,
      };
      await _dbContext.InsertAsync(newEventBroker, cancellationToken);

      foreach (var item in request.Location)
      {
        var newEventLocationBroker = new MsEventLocationBroker
        {
          Id = new UuidV7().Value,
          Location = item,
          EventBrokerId = newEventBroker.Id
        };
        await _dbContext.InsertAsync(newEventLocationBroker, cancellationToken);
      }

      await _dbContext.SaveChangesAsync(cancellationToken);

      #region MessageBroker
      var getEventBroker = await _dbContext.Set<MsEventBroker>()
               .Include(e => e.EventLocationBroker)
               .Where(e => e.Id == newEventBroker.Id)
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
        Message = JsonSerializer.Serialize(getEventBroker),
        Scenario = "CreateEvent",
        QueueName = "event"
      };

      _emailQueue.SendQueueAsync(_paramQueue);
      #endregion

      return NoContent();
    }
  }
}
