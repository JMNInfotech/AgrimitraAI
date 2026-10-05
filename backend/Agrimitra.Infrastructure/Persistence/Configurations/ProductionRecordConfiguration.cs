using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ProductionRecordConfiguration : IEntityTypeConfiguration<ProductionRecord>
{
    public void Configure(EntityTypeBuilder<ProductionRecord> entity)
    {
        entity.HasKey(e => e.Id).HasName("production_records_pkey");

        entity.ToTable("production_records");

        entity.HasIndex(e => e.CropId, "ix_production_crop");

        entity.HasIndex(e => e.CropCycleId, "ix_production_cycle");

        entity.HasIndex(e => new { e.FarmerProfileId, e.ProductionDate }, "ix_production_farmer_date")
            .IsDescending(false, true)
            .HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.HarvestRecordId, "ix_production_harvest");

        entity.HasIndex(e => e.LandId, "ix_production_land");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.HarvestRecordId).HasColumnName("harvest_record_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.ProductionDate).HasColumnName("production_date");
        entity.Property(e => e.QualityGrade).HasColumnName("quality_grade");
        entity.Property(e => e.Quantity)
            .HasPrecision(14, 3)
            .HasColumnName("quantity");
        entity.Property(e => e.Unit).HasColumnName("unit");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.ProductionRecords)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("production_records_crop_cycle_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.ProductionRecords)
            .HasForeignKey(d => d.CropId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("production_records_crop_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.ProductionRecords)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("production_records_farmer_profile_id_fkey");

        entity.HasOne(d => d.HarvestRecord).WithMany(p => p.ProductionRecords)
            .HasForeignKey(d => d.HarvestRecordId)
            .HasConstraintName("production_records_harvest_record_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.ProductionRecords)
            .HasForeignKey(d => d.LandId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("production_records_land_id_fkey");
    }
}
