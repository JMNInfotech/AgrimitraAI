using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class SystemConfigurationConfiguration : IEntityTypeConfiguration<SystemConfiguration>
{
    public void Configure(EntityTypeBuilder<SystemConfiguration> entity)
    {
        entity.HasKey(e => e.Id).HasName("system_configurations_pkey");

        entity.ToTable("system_configurations");

        entity.HasIndex(e => e.ConfigKey, "system_configurations_config_key_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ConfigKey).HasColumnName("config_key");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.IsSensitive).HasColumnName("is_sensitive");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.Value)
            .HasColumnType("jsonb")
            .HasColumnName("value");
        entity.Property(e => e.ValueType)
            .HasDefaultValueSql("'string'::text")
            .HasColumnName("value_type");
    }
}
