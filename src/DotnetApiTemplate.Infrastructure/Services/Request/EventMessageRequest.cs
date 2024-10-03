namespace DotnetApiTemplate.Infrastructure.Services.Request
{
  public class EventMessageRequest 
  {
    public Guid EventId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CountTicket { get; set; }
    public List<EventLocationRequest> Location { get; set; } = new List<EventLocationRequest>();
  }

  public class EventLocationRequest
  {
    public Guid EventLocationId { get; set; }
    public Guid EventId { get; set; }
    public string Location { get; set; } = string.Empty;
  }
}
