using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropDiseaseHistoryConfiguration : IEntityTypeConfiguration<CropDiseaseHistory>
{
    public void Configure(EntityTypeBuilder<CropDiseaseHistory> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_disease_history_pkey");

        entity.ToTable("crop_disease_history");

        entity.HasIndex(e => new { e.CropCycleId, e.DetectedOn }, "ix_crop_disease_history_cycle").IsDescending(false, true);

        entity.HasIndex(e => e.CropDiseaseId, "ix_crop_disease_history_disease");

        entity.HasIndex(e => e.ConsultationId, "ix_disease_history_consultation");

        entity.HasIndex(e => e.DiseaseScanId, "ix_disease_history_scan");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.CropDiseaseId).HasColumnName("crop_disease_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.DetectedOn).HasColumnName("detected_on");
        entity.Property(e => e.DiseaseScanId).HasColumnName("disease_scan_id");
        entity.Property(e => e.IsConfirmed).HasColumnName("is_confirmed");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.ResolvedOn).HasColumnName("resolved_on");
        entity.Property(e => e.Severity).HasColumnName("severity");
        entity.Property(e => e.Source).HasColumnName("source");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Consultation).WithMany(p => p.CropDiseaseHistories)
            .HasForeignKey(d => d.ConsultationId)
            .HasConstraintName("fk_disease_history_consultation");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.CropDiseaseHistories)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("crop_disease_history_crop_cycle_id_fkey");

        entity.HasOne(d => d.CropDisease).WithMany(p => p.CropDiseaseHistories)
            .HasForeignKey(d => d.CropDiseaseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_disease_history_crop_disease_id_fkey");

        entity.HasOne(d => d.DiseaseScan).WithMany(p => p.CropDiseaseHistories)
            .HasForeignKey(d => d.DiseaseScanId)
            .HasConstraintName("fk_disease_history_scan");
    }
}
