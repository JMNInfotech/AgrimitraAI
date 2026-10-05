using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AiModelVersionConfiguration : IEntityTypeConfiguration<AiModelVersion>
{
    public void Configure(EntityTypeBuilder<AiModelVersion> entity)
    {
        entity.HasKey(e => e.Id).HasName("ai_model_versions_pkey");

        entity.ToTable("ai_model_versions");

        entity.HasIndex(e => new { e.ModelId, e.Version }, "ai_model_versions_model_id_version_key").IsUnique();

        entity.HasIndex(e => e.ApprovedBy, "ix_ai_model_versions_approved_by_de4e9a");

        entity.HasIndex(e => e.DatasetVersionId, "ix_model_versions_dataset");

        entity.HasIndex(e => new { e.ModelId, e.Environment }, "ux_model_versions_production")
            .IsUnique()
            .HasFilter("(status = 'production'::text)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Accuracy)
            .HasPrecision(6, 5)
            .HasColumnName("accuracy");
        entity.Property(e => e.ApprovedAt).HasColumnName("approved_at");
        entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
        entity.Property(e => e.ArtifactSha256).HasColumnName("artifact_sha256");
        entity.Property(e => e.ArtifactUri).HasColumnName("artifact_uri");
        entity.Property(e => e.ConfidenceThreshold)
            .HasPrecision(4, 3)
            .HasDefaultValue(0.700m)
            .HasColumnName("confidence_threshold");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DatasetVersionId).HasColumnName("dataset_version_id");
        entity.Property(e => e.DeployedAt).HasColumnName("deployed_at");
        entity.Property(e => e.Environment)
            .HasDefaultValueSql("'dev'::text")
            .HasColumnName("environment");
        entity.Property(e => e.F1Score)
            .HasPrecision(6, 5)
            .HasColumnName("f1_score");
        entity.Property(e => e.ModelId).HasColumnName("model_id");
        entity.Property(e => e.PrecisionScore)
            .HasPrecision(6, 5)
            .HasColumnName("precision_score");
        entity.Property(e => e.RecallScore)
            .HasPrecision(6, 5)
            .HasColumnName("recall_score");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'draft'::text")
            .HasColumnName("status");
        entity.Property(e => e.TrainedAt).HasColumnName("trained_at");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.Version).HasColumnName("version");

        entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.AiModelVersions)
            .HasForeignKey(d => d.ApprovedBy)
            .HasConstraintName("ai_model_versions_approved_by_fkey");

        entity.HasOne(d => d.DatasetVersion).WithMany(p => p.AiModelVersions)
            .HasForeignKey(d => d.DatasetVersionId)
            .HasConstraintName("ai_model_versions_dataset_version_id_fkey");

        entity.HasOne(d => d.Model).WithMany(p => p.AiModelVersions)
            .HasForeignKey(d => d.ModelId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ai_model_versions_model_id_fkey");
    }
}
