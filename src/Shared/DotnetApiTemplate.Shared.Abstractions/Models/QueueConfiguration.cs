using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Shared.Abstractions.Models
{
  public class QueueConfiguration
  {
    public string Connection { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
  }
}
