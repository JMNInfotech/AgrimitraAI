using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class DiseaseResultConfiguration : IEntityTypeConfiguration<DiseaseResult>
{
    public void Configure(EntityTypeBuilder<DiseaseResult> entity)
    {
        entity.HasKey(e => e.Id).HasName("disease_results_pkey");

        entity.ToTable("disease_results");

        entity.HasIndex(e => new { e.DiseaseScanId, e.Rank }, "disease_results_disease_scan_id_rank_key").IsUnique();

        entity.HasIndex(e => e.DatasetVersionId, "ix_disease_results_dataset_version_id_7ee533");

        entity.HasIndex(e => e.DetectedCropId, "ix_disease_results_detected_crop_id_7773b5");

        entity.HasIndex(e => e.CropDiseaseId, "ix_disease_results_disease");

        entity.HasIndex(e => e.ExplanationFileId, "ix_disease_results_explanation_file_id_b13483");

        entity.HasIndex(e => e.ModelVersionId, "ix_disease_results_model");

        entity.HasIndex(e => e.CreatedAt, "ix_disease_results_review").HasFilter("needs_expert_review");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AffectedAreaPercent)
            .HasPrecision(5, 2)
            .HasColumnName("affected_area_percent");
        entity.Property(e => e.Confidence)
            .HasPrecision(6, 5)
            .HasColumnName("confidence");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropDiseaseId).HasColumnName("crop_disease_id");
        entity.Property(e => e.DatasetVersionId).HasColumnName("dataset_version_id");
        entity.Property(e => e.DetectedCropId).HasColumnName("detected_crop_id");
        entity.Property(e => e.Disclaimer).HasColumnName("disclaimer");
        entity.Property(e => e.DiseaseScanId).HasColumnName("disease_scan_id");
        entity.Property(e => e.ExplanationFileId).HasColumnName("explanation_file_id");
        entity.Property(e => e.InferenceLogCreatedAt).HasColumnName("inference_log_created_at");
        entity.Property(e => e.InferenceLogId).HasColumnName("inference_log_id");
        entity.Property(e => e.ModelVersionId).HasColumnName("model_version_id");
        entity.Property(e => e.NeedsExpertReview).HasColumnName("needs_expert_review");
        entity.Property(e => e.Prevention).HasColumnName("prevention");
        entity.Property(e => e.Rank)
            .HasDefaultValue((short)1)
            .HasColumnName("rank");
        entity.Property(e => e.RecommendedAction).HasColumnName("recommended_action");
        entity.Property(e => e.Severity).HasColumnName("severity");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.CropDisease).WithMany(p => p.DiseaseResults)
            .HasForeignKey(d => d.CropDiseaseId)
            .HasConstraintName("disease_results_crop_disease_id_fkey");

        entity.HasOne(d => d.DatasetVersion).WithMany(p => p.DiseaseResults)
            .HasForeignKey(d => d.DatasetVersionId)
            .HasConstraintName("disease_results_dataset_version_id_fkey");

        entity.HasOne(d => d.DetectedCrop).WithMany(p => p.DiseaseResults)
            .HasForeignKey(d => d.DetectedCropId)
            .HasConstraintName("disease_results_detected_crop_id_fkey");

        entity.HasOne(d => d.DiseaseScan).WithMany(p => p.DiseaseResults)
            .HasForeignKey(d => d.DiseaseScanId)
            .HasConstraintName("disease_results_disease_scan_id_fkey");

        entity.HasOne(d => d.ExplanationFile).WithMany(p => p.DiseaseResults)
            .HasForeignKey(d => d.ExplanationFileId)
            .HasConstraintName("disease_results_explanation_file_id_fkey");

        entity.HasOne(d => d.ModelVersion).WithMany(p => p.DiseaseResults)
            .HasForeignKey(d => d.ModelVersionId)
            .HasConstraintName("disease_results_model_version_id_fkey");
    }
}
