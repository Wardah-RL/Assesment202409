using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.Event.Request
{
  public class DeleteEventRequest
  {
    [FromRoute(Name = "eventId")] public Guid EventId { get; set; }
  }
}
