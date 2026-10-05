using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropCycleConfiguration : IEntityTypeConfiguration<CropCycle>
{
    public void Configure(EntityTypeBuilder<CropCycle> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_cycles_pkey");

        entity.ToTable("crop_cycles");

        entity.HasIndex(e => e.CropId, "ix_crop_cycles_crop");

        entity.HasIndex(e => new { e.FarmerProfileId, e.Status }, "ix_crop_cycles_farmer_status").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.ExpectedHarvestDate, "ix_crop_cycles_harvest").HasFilter("(status = 'active'::text)");

        entity.HasIndex(e => e.IrrigationTypeId, "ix_crop_cycles_irrigation_type_id_87717f");

        entity.HasIndex(e => new { e.LandId, e.Status }, "ix_crop_cycles_land_status").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.CurrentStageId, "ix_crop_cycles_stage");

        entity.HasIndex(e => e.VarietyId, "ix_crop_cycles_variety");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActualHarvestDate).HasColumnName("actual_harvest_date");
        entity.Property(e => e.AreaUnit).HasColumnName("area_unit");
        entity.Property(e => e.AreaValue)
            .HasPrecision(14, 4)
            .HasColumnName("area_value");
        entity.Property(e => e.ClosedAt).HasColumnName("closed_at");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.CurrentStageId).HasColumnName("current_stage_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.ExpectedHarvestDate).HasColumnName("expected_harvest_date");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IrrigationTypeId).HasColumnName("irrigation_type_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.PlantCount).HasColumnName("plant_count");
        entity.Property(e => e.PlantingDate).HasColumnName("planting_date");
        entity.Property(e => e.QualityGrade).HasColumnName("quality_grade");
        entity.Property(e => e.Season).HasColumnName("season");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'active'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.VarietyId).HasColumnName("variety_id");

        entity.HasOne(d => d.Crop).WithMany(p => p.CropCycles)
            .HasForeignKey(d => d.CropId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_cycles_crop_id_fkey");

        entity.HasOne(d => d.CurrentStage).WithMany(p => p.CropCycles)
            .HasForeignKey(d => d.CurrentStageId)
            .HasConstraintName("crop_cycles_current_stage_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.CropCycles)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_cycles_farmer_profile_id_fkey");

        entity.HasOne(d => d.IrrigationType).WithMany(p => p.CropCycles)
            .HasForeignKey(d => d.IrrigationTypeId)
            .HasConstraintName("crop_cycles_irrigation_type_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.CropCycles)
            .HasForeignKey(d => d.LandId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_cycles_land_id_fkey");

        entity.HasOne(d => d.Variety).WithMany(p => p.CropCycles)
            .HasForeignKey(d => d.VarietyId)
            .HasConstraintName("crop_cycles_variety_id_fkey");
    }
}
