using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Queries;
using DotnetApiTemplate.WebApi.Contracts.Responses;
using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq.Dynamic.Core;

namespace DotnetApiTemplate.WebApi.Endpoints.Event
{
  public class GetEvent : BaseEndpoint<GetAllMenuPaginatedRequest, PagedList<GetEventResponse>>
  {
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<GetEvent> _localizer;

    public GetEvent(IDbContext dbContext,
        IStringLocalizer<GetEvent> localizer)
    {
      _dbContext = dbContext;
      _localizer = localizer;
    }

    [HttpGet("event")]
    [Authorize]
    [SwaggerOperation(
       Summary = "Get event API",
       Description = "",
       OperationId = "Event.GetEvent",
       Tags = new[] { "Event" })
   ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<PagedList<GetEventResponse>>> HandleAsync([FromQuery] GetAllMenuPaginatedRequest request,
        CancellationToken cancellationToken = new())
    {
      var queryable = _dbContext.Set<MsEvent>()
                      .AsQueryable();

      var totalRows = await queryable.CountAsync(cancellationToken);

      if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
        queryable = queryable.Where(e => e.Name.ToLower().Contains(request.Search.ToLower()));

      if (!string.IsNullOrWhiteSpace(request.OrderBy) && !string.IsNullOrWhiteSpace(request.OrderType))
        queryable = queryable.OrderBy(request.OrderType, request.OrderBy);
      else
        queryable = queryable.OrderBy("Name", "ASC");

      var listEvent = queryable
          .Select(e => new 
          {
            EventId = e.Id,
            Name = e.Name,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            CountTicket = e.CountTicket,
            Location = e.EventLocation,
          })
          .Skip(request.CalculateSkip())
          .ToList();

      var getEvent = listEvent
          .Select(e => new GetEventResponse
          {
            EventId = e.EventId,
            Name = e.Name,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            CountTicket = e.CountTicket,
            Location = String.Join(",", e.Location.Select(f=>f.Location).ToList()),
          })
          .ToList();

      var totalRowFilter = getEvent.Count;

      var response = new PagedList<GetEventResponse>(getEvent, request.Page, request.Size);

      return response;
    }
  }
}
