using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class DiseaseScanConfiguration : IEntityTypeConfiguration<DiseaseScan>
{
    public void Configure(EntityTypeBuilder<DiseaseScan> entity)
    {
        entity.HasKey(e => e.Id).HasName("disease_scans_pkey");

        entity.ToTable("disease_scans");

        entity.HasIndex(e => e.ClientMutationId, "disease_scans_client_mutation_id_key").IsUnique();

        entity.HasIndex(e => e.UserId, "ix_disease_scans_user_id_f07276");

        entity.HasIndex(e => e.CropId, "ix_scans_crop");

        entity.HasIndex(e => e.CropCycleId, "ix_scans_cycle");

        entity.HasIndex(e => new { e.FarmerProfileId, e.CreatedAt }, "ix_scans_farmer")
            .IsDescending(false, true)
            .HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.LandId, "ix_scans_land");

        entity.HasIndex(e => e.Status, "ix_scans_status").HasFilter("(status = ANY (ARRAY['queued'::text, 'processing'::text]))");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CapturedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("captured_at");
        entity.Property(e => e.ClientMutationId).HasColumnName("client_mutation_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.Location)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("location");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'queued'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.DiseaseScans)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("disease_scans_crop_cycle_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.DiseaseScans)
            .HasForeignKey(d => d.CropId)
            .HasConstraintName("disease_scans_crop_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.DiseaseScans)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("disease_scans_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.DiseaseScans)
            .HasForeignKey(d => d.LandId)
            .HasConstraintName("disease_scans_land_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.DiseaseScans)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("disease_scans_user_id_fkey");
    }
}
