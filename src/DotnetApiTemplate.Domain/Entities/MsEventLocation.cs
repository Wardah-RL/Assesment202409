using DotnetApiTemplate.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Domain.Entities
{
  public class MsEventLocation : BaseEntity
  {
    public string Location { get; set; } = null!;

    [ForeignKey(nameof(Event))]
    public Guid EventId { get; set; }
    public MsEvent? Event { get; set; }
  }
}
