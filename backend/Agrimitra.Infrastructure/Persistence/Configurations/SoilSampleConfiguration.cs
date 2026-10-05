using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class SoilSampleConfiguration : IEntityTypeConfiguration<SoilSample>
{
    public void Configure(EntityTypeBuilder<SoilSample> entity)
    {
        entity.HasKey(e => e.Id).HasName("soil_samples_pkey");

        entity.ToTable("soil_samples");

        entity.HasIndex(e => e.CropCycleId, "ix_soil_samples_cycle");

        entity.HasIndex(e => e.FarmerProfileId, "ix_soil_samples_farmer");

        entity.HasIndex(e => new { e.LandId, e.CollectedOn }, "ix_soil_samples_land")
            .IsDescending(false, true)
            .HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.SampleCode, "soil_samples_sample_code_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CollectedBy).HasColumnName("collected_by");
        entity.Property(e => e.CollectedOn).HasColumnName("collected_on");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.DepthCm)
            .HasPrecision(6, 1)
            .HasColumnName("depth_cm");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.Location)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("location");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.SampleCode).HasColumnName("sample_code");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.SoilSamples)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("soil_samples_crop_cycle_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.SoilSamples)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("soil_samples_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.SoilSamples)
            .HasForeignKey(d => d.LandId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("soil_samples_land_id_fkey");
    }
}
