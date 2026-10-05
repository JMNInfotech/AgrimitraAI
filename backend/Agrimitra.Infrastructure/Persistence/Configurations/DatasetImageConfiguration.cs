using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class DatasetImageConfiguration : IEntityTypeConfiguration<DatasetImage>
{
    public void Configure(EntityTypeBuilder<DatasetImage> entity)
    {
        entity.HasKey(e => e.Id).HasName("dataset_images_pkey");

        entity.ToTable("dataset_images");

        entity.HasIndex(e => new { e.DatasetVersionId, e.FileObjectId }, "dataset_images_dataset_version_id_file_object_id_key").IsUnique();

        entity.HasIndex(e => e.CropId, "ix_dataset_images_crop");

        entity.HasIndex(e => e.CropDiseaseId, "ix_dataset_images_disease");

        entity.HasIndex(e => e.FileObjectId, "ix_dataset_images_file_object_id_1bcf37");

        entity.HasIndex(e => e.LabeledBy, "ix_dataset_images_labeled_by_c824e0");

        entity.HasIndex(e => e.ReviewedBy, "ix_dataset_images_reviewed_by_811522");

        entity.HasIndex(e => new { e.DatasetVersionId, e.Split }, "ix_dataset_images_version_split");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Annotation)
            .HasColumnType("jsonb")
            .HasColumnName("annotation");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropDiseaseId).HasColumnName("crop_disease_id");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.DatasetVersionId).HasColumnName("dataset_version_id");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.Label).HasColumnName("label");
        entity.Property(e => e.LabeledBy).HasColumnName("labeled_by");
        entity.Property(e => e.ReviewedBy).HasColumnName("reviewed_by");
        entity.Property(e => e.Source).HasColumnName("source");
        entity.Property(e => e.Split).HasColumnName("split");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.CropDisease).WithMany(p => p.DatasetImages)
            .HasForeignKey(d => d.CropDiseaseId)
            .HasConstraintName("dataset_images_crop_disease_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.DatasetImages)
            .HasForeignKey(d => d.CropId)
            .HasConstraintName("dataset_images_crop_id_fkey");

        entity.HasOne(d => d.DatasetVersion).WithMany(p => p.DatasetImages)
            .HasForeignKey(d => d.DatasetVersionId)
            .HasConstraintName("dataset_images_dataset_version_id_fkey");

        entity.HasOne(d => d.FileObject).WithMany(p => p.DatasetImages)
            .HasForeignKey(d => d.FileObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("dataset_images_file_object_id_fkey");

        entity.HasOne(d => d.LabeledByNavigation).WithMany(p => p.DatasetImageLabeledByNavigations)
            .HasForeignKey(d => d.LabeledBy)
            .HasConstraintName("dataset_images_labeled_by_fkey");

        entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.DatasetImageReviewedByNavigations)
            .HasForeignKey(d => d.ReviewedBy)
            .HasConstraintName("dataset_images_reviewed_by_fkey");
    }
}
