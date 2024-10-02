using DotnetApiTemplate.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Domain.Entities
{
  public class MsEvent : BaseEntity
  {
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Lokasi { get; set; } = null!;
    public string JumlahTiket { get; set; } = null!;
  }
}
