using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models;
using DotnetApiTemplate.Domain.Entities;
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

namespace DotnetApiTemplate.WebApi.Endpoints.Event
{
  public class CreateEvent : BaseEndpointWithoutResponse<CreateEventRequest>
  {
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<CreateEvent> _localizer;
    private readonly ISendQueue _emailQueue;
    private SendQueueRequest _paramQueue = new SendQueueRequest();
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
    [RequiredScope(typeof(UserManagementScope))]
    [SwaggerOperation(
        Summary = "Create user API",
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

      var eventBroker = new MsEventBroker
      {
        Id = new UuidV7().Value,
        Name = request.Name,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
        Lokasi = request.Lokasi,
        JumlahTiket = request.JumlahTiket,
      };

      await _dbContext.InsertAsync(eventBroker, cancellationToken);
      await _dbContext.SaveChangesAsync(cancellationToken);

      #region MessageBroker
      var getEventBroker = await _dbContext.Set<MsEventBroker>()
               .Where(e => e.Id == eventBroker.Id)
               .FirstOrDefaultAsync(cancellationToken);

      _paramQueue.Scope = "Event";
      _paramQueue.Scenario = "CreateEvent";

      if (_paramQueue.KeyValues.ContainsKey("createEvent"))
        _paramQueue.KeyValues.Remove("createEvent");

      _paramQueue.KeyValues.Add("createEvent", getEventBroker);

      _emailQueue.SendQueueAsync(_paramQueue);
      #endregion

      return NoContent();
    }
  }
}
