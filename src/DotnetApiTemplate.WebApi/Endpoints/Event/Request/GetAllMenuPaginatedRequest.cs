using DotnetApiTemplate.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.Event.Request
{
  public class GetAllMenuPaginatedRequest : BasePaginationCalculation
  {
    [FromQuery(Name = "search")] public string? Search { get; set; }
  }
}
