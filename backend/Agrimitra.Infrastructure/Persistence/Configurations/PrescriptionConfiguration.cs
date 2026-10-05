using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> entity)
    {
        entity.HasKey(e => e.Id).HasName("prescriptions_pkey");

        entity.ToTable("prescriptions");

        entity.HasIndex(e => new { e.ConsultantId, e.Status }, "ix_prescriptions_consultant");

        entity.HasIndex(e => e.ConsultationId, "ix_prescriptions_consultation");

        entity.HasIndex(e => e.CropCycleId, "ix_prescriptions_cycle");

        entity.HasIndex(e => new { e.FarmerProfileId, e.CreatedAt }, "ix_prescriptions_farmer")
            .IsDescending(false, true)
            .HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.FollowUpDate, "ix_prescriptions_followup").HasFilter("(follow_up_date IS NOT NULL)");

        entity.HasIndex(e => e.LandId, "ix_prescriptions_land");

        entity.HasIndex(e => e.PdfFileObjectId, "ix_prescriptions_pdf_file_object_id_6a6191");

        entity.HasIndex(e => e.SupersedesId, "ix_prescriptions_supersedes");

        entity.HasIndex(e => e.PrescriptionNumber, "prescriptions_prescription_number_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Advisory).HasColumnName("advisory");
        entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
        entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Diagnosis).HasColumnName("diagnosis");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.FollowUpDate).HasColumnName("follow_up_date");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.IssuedAt).HasColumnName("issued_at");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.PdfFileObjectId).HasColumnName("pdf_file_object_id");
        entity.Property(e => e.Precautions).HasColumnName("precautions");
        entity.Property(e => e.PrescriptionNumber).HasColumnName("prescription_number");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'draft'::text")
            .HasColumnName("status");
        entity.Property(e => e.SupersedesId).HasColumnName("supersedes_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.Version)
            .HasDefaultValue(1)
            .HasColumnName("version");

        entity.HasOne(d => d.Consultant).WithMany(p => p.Prescriptions)
            .HasForeignKey(d => d.ConsultantId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("prescriptions_consultant_id_fkey");

        entity.HasOne(d => d.Consultation).WithMany(p => p.Prescriptions)
            .HasForeignKey(d => d.ConsultationId)
            .HasConstraintName("prescriptions_consultation_id_fkey");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.Prescriptions)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("prescriptions_crop_cycle_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.Prescriptions)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("prescriptions_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.Prescriptions)
            .HasForeignKey(d => d.LandId)
            .HasConstraintName("prescriptions_land_id_fkey");

        entity.HasOne(d => d.PdfFileObject).WithMany(p => p.Prescriptions)
            .HasForeignKey(d => d.PdfFileObjectId)
            .HasConstraintName("prescriptions_pdf_file_object_id_fkey");

        entity.HasOne(d => d.Supersedes).WithMany(p => p.InverseSupersedes)
            .HasForeignKey(d => d.SupersedesId)
            .HasConstraintName("prescriptions_supersedes_id_fkey");
    }
}
