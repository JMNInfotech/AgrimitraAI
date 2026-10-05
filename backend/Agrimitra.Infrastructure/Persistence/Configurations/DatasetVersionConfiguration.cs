using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class DatasetVersionConfiguration : IEntityTypeConfiguration<DatasetVersion>
{
    public void Configure(EntityTypeBuilder<DatasetVersion> entity)
    {
        entity.HasKey(e => e.Id).HasName("dataset_versions_pkey");

        entity.ToTable("dataset_versions");

        entity.HasIndex(e => new { e.DatasetId, e.Version }, "dataset_versions_dataset_id_version_key").IsUnique();

        entity.HasIndex(e => e.ApprovedBy, "ix_dataset_versions_approved_by_8ca7df");

        entity.HasIndex(e => e.ManifestFileId, "ix_dataset_versions_manifest_file_id_43b68d");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ApprovedAt).HasColumnName("approved_at");
        entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DatasetId).HasColumnName("dataset_id");
        entity.Property(e => e.ImageCount).HasColumnName("image_count");
        entity.Property(e => e.ManifestFileId).HasColumnName("manifest_file_id");
        entity.Property(e => e.SplitSeed).HasColumnName("split_seed");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'draft'::text")
            .HasColumnName("status");
        entity.Property(e => e.TestCount).HasColumnName("test_count");
        entity.Property(e => e.TrainCount).HasColumnName("train_count");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.ValidationCount).HasColumnName("validation_count");
        entity.Property(e => e.Version).HasColumnName("version");

        entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.DatasetVersions)
            .HasForeignKey(d => d.ApprovedBy)
            .HasConstraintName("dataset_versions_approved_by_fkey");

        entity.HasOne(d => d.Dataset).WithMany(p => p.DatasetVersions)
            .HasForeignKey(d => d.DatasetId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("dataset_versions_dataset_id_fkey");

        entity.HasOne(d => d.ManifestFile).WithMany(p => p.DatasetVersions)
            .HasForeignKey(d => d.ManifestFileId)
            .HasConstraintName("dataset_versions_manifest_file_id_fkey");
    }
}
