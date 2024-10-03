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
  public class MsEventLocationBrokerConfiguration : BaseEntityConfiguration<MsEventLocationBroker>
  {
    protected override void EntityConfiguration(EntityTypeBuilder<MsEventLocationBroker> builder)
    {
      builder.Property(e => e.Location).HasMaxLength(512);
    }
  }
}
