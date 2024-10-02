using DotnetApiTemplate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Core.Models
{
  public class SendQueueRequest
  {
    public IDictionary<string, object> KeyValues { get; set; } = new Dictionary<string, object>();
    public string Scope { get; set; } = null!;
    public string Scenario { get; set; } = null!;
  }
}
