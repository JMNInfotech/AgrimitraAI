using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AdPlacementConfiguration : IEntityTypeConfiguration<AdPlacement>
{
    public void Configure(EntityTypeBuilder<AdPlacement> entity)
    {
        entity.HasKey(e => e.Id).HasName("ad_placements_pkey");

        entity.ToTable("ad_placements");

        entity.HasIndex(e => e.Code, "ad_placements_code_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AllowedAdTypes)
            .HasDefaultValueSql("'{}'::text[]")
            .HasColumnName("allowed_ad_types");
        entity.Property(e => e.BasePriceCpm)
            .HasPrecision(12, 4)
            .HasColumnName("base_price_cpm");
        entity.Property(e => e.Code).HasColumnName("code");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        entity.Property(e => e.MaxItems)
            .HasDefaultValue(1)
            .HasColumnName("max_items");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
    }
}
