using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropProductionConfiguration : IEntityTypeConfiguration<CropProduction>
{
    public void Configure(EntityTypeBuilder<CropProduction> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_production_pkey");

        entity.ToTable("crop_production");

        entity.HasIndex(e => e.CropCycleId, "crop_production_crop_cycle_id_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActualQuantity)
            .HasPrecision(14, 3)
            .HasColumnName("actual_quantity");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.ExpectedQuantity)
            .HasPrecision(14, 3)
            .HasColumnName("expected_quantity");
        entity.Property(e => e.MarketPricePerUnit)
            .HasPrecision(12, 2)
            .HasColumnName("market_price_per_unit");
        entity.Property(e => e.QualityGrade).HasColumnName("quality_grade");
        entity.Property(e => e.Unit).HasColumnName("unit");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.CropCycle).WithOne(p => p.CropProduction)
            .HasForeignKey<CropProduction>(d => d.CropCycleId)
            .HasConstraintName("crop_production_crop_cycle_id_fkey");
    }
}
