using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Persistence.Postgres.Configurations
{

  public class MsTicketConfiguration : BaseEntityConfiguration<MsTicket>
  {
    protected override void EntityConfiguration(EntityTypeBuilder<MsTicket> builder)
    {
      builder.Property(e => e.TrainName).HasMaxLength(256);
    }
  }
}
