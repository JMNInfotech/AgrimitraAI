using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsultantCropCarePlanConfiguration : IEntityTypeConfiguration<ConsultantCropCarePlan>
{
    public void Configure(EntityTypeBuilder<ConsultantCropCarePlan> entity)
    {
        entity.HasKey(e => e.Id).HasName("consultant_crop_care_plans_pkey");

        entity.ToTable("consultant_crop_care_plans");

        entity.HasIndex(e => new { e.ConsultantId, e.Status }, "ix_care_plans_consultant").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.ConsultationId, "ix_care_plans_consultation");

        entity.HasIndex(e => e.CropId, "ix_care_plans_crop");

        entity.HasIndex(e => e.CropCycleId, "ix_care_plans_cycle");

        entity.HasIndex(e => new { e.FarmerProfileId, e.Status }, "ix_care_plans_farmer").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.LandId, "ix_care_plans_land");

        entity.HasIndex(e => e.PrescriptionId, "ix_care_plans_prescription");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
        entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.FollowUpDate).HasColumnName("follow_up_date");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");
        entity.Property(e => e.PublishedAt).HasColumnName("published_at");
        entity.Property(e => e.PublishedVersion).HasColumnName("published_version");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'draft'::text")
            .HasColumnName("status");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Consultant).WithMany(p => p.ConsultantCropCarePlans)
            .HasForeignKey(d => d.ConsultantId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_crop_care_plans_consultant_id_fkey");

        entity.HasOne(d => d.Consultation).WithMany(p => p.ConsultantCropCarePlans)
            .HasForeignKey(d => d.ConsultationId)
            .HasConstraintName("consultant_crop_care_plans_consultation_id_fkey");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.ConsultantCropCarePlans)
            .HasForeignKey(d => d.CropCycleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_crop_care_plans_crop_cycle_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.ConsultantCropCarePlans)
            .HasForeignKey(d => d.CropId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_crop_care_plans_crop_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.ConsultantCropCarePlans)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_crop_care_plans_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.ConsultantCropCarePlans)
            .HasForeignKey(d => d.LandId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_crop_care_plans_land_id_fkey");

        entity.HasOne(d => d.Prescription).WithMany(p => p.ConsultantCropCarePlans)
            .HasForeignKey(d => d.PrescriptionId)
            .HasConstraintName("consultant_crop_care_plans_prescription_id_fkey");
    }
}
