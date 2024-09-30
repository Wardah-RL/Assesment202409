using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DotnetApiTemplate.Shared.Abstractions.Entities;

public abstract class BaseEntityConfiguration<TBaseEntity> : IEntityTypeConfiguration<TBaseEntity>
    where TBaseEntity : BaseEntity
{
    public void Configure(EntityTypeBuilder<TBaseEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.CreatedBy).HasMaxLength(maxLength: 256);
        builder.Property(e => e.CreatedByName).HasMaxLength(maxLength: 256);
        builder.Property(e => e.CreatedByFullName).HasMaxLength(maxLength: 512);

        builder.Property(e => e.LastUpdatedBy).HasMaxLength(maxLength: 256);
        builder.Property(e => e.LastUpdatedByName).HasMaxLength(maxLength: 256);
        builder.Property(e => e.LastUpdatedByFullName).HasMaxLength(maxLength: 512);

        builder.HasQueryFilter(e => e.IsDeleted == false);

        builder.HasIndex(e => e.CreatedByName);
        builder.HasIndex(e => e.CreatedByFullName);

        builder.HasIndex(e => e.LastUpdatedBy);
        builder.HasIndex(e => e.LastUpdatedByName);
        builder.HasIndex(e => e.LastUpdatedByFullName);

        EntityConfiguration(builder);
    }

    protected abstract void EntityConfiguration(EntityTypeBuilder<TBaseEntity> builder);

}