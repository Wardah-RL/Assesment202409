using DotnetApiTemplate.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Domain.Entities
{
  public class MsEventLocationBroker : BaseEntity
  {
    public string Location { get; set; } = null!;

    [ForeignKey(nameof(EventBroker))]
    public Guid EventBrokerId { get; set; }
    public MsEventBroker? EventBroker { get; set; }
  }
}
