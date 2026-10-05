using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class HarvestRecordConfiguration : IEntityTypeConfiguration<HarvestRecord>
{
    public void Configure(EntityTypeBuilder<HarvestRecord> entity)
    {
        entity.HasKey(e => e.Id).HasName("harvest_records_pkey");

        entity.ToTable("harvest_records");

        entity.HasIndex(e => new { e.CropCycleId, e.HarvestDate }, "ix_harvest_cycle")
            .IsDescending(false, true)
            .HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.LandId, "ix_harvest_land");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.HarvestDate).HasColumnName("harvest_date");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LabourCount).HasColumnName("labour_count");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.QualityGrade).HasColumnName("quality_grade");
        entity.Property(e => e.Quantity)
            .HasPrecision(14, 3)
            .HasColumnName("quantity");
        entity.Property(e => e.Unit).HasColumnName("unit");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.HarvestRecords)
            .HasForeignKey(d => d.CropCycleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("harvest_records_crop_cycle_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.HarvestRecords)
            .HasForeignKey(d => d.LandId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("harvest_records_land_id_fkey");
    }
}
