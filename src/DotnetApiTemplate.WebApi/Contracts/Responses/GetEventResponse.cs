using DotnetApiTemplate.WebApi.Endpoints.Event.Request;

namespace DotnetApiTemplate.WebApi.Contracts.Responses
{
  public class GetEventResponse 
  {
    public Guid EventId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CountTicket { get; set; }
    public string Location { get; set; }
  }
}
