namespace DotnetApiTemplate.WebApi.Endpoints.Event.Request
{
  public class CreateEventRequest
  {
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CountTicket { get; set; }
    public List<string> Location {  get; set; }

  }
}
