using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropActivityEvidenceConfiguration : IEntityTypeConfiguration<CropActivityEvidence>
{
    public void Configure(EntityTypeBuilder<CropActivityEvidence> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_activity_evidence_pkey");

        entity.ToTable("crop_activity_evidence");

        entity.HasIndex(e => e.FileObjectId, "ix_crop_activity_evidence_file_object_id_a51119");

        entity.HasIndex(e => e.UploadedBy, "ix_crop_activity_evidence_uploaded_by_2e593c");

        entity.HasIndex(e => e.CompletionId, "ix_evidence_completion");

        entity.HasIndex(e => e.OccurrenceId, "ix_evidence_occurrence").HasFilter("(NOT is_deleted)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Caption).HasColumnName("caption");
        entity.Property(e => e.CapturedAt).HasColumnName("captured_at");
        entity.Property(e => e.CompletionId).HasColumnName("completion_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.Location)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("location");
        entity.Property(e => e.OccurrenceId).HasColumnName("occurrence_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UploadedBy).HasColumnName("uploaded_by");

        entity.HasOne(d => d.Completion).WithMany(p => p.CropActivityEvidences)
            .HasForeignKey(d => d.CompletionId)
            .HasConstraintName("crop_activity_evidence_completion_id_fkey");

        entity.HasOne(d => d.FileObject).WithMany(p => p.CropActivityEvidences)
            .HasForeignKey(d => d.FileObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_activity_evidence_file_object_id_fkey");

        entity.HasOne(d => d.Occurrence).WithMany(p => p.CropActivityEvidences)
            .HasForeignKey(d => d.OccurrenceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_activity_evidence_occurrence_id_fkey");

        entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.CropActivityEvidences)
            .HasForeignKey(d => d.UploadedBy)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_activity_evidence_uploaded_by_fkey");
    }
}
