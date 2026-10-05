using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AiModelConfiguration : IEntityTypeConfiguration<AiModel>
{
    public void Configure(EntityTypeBuilder<AiModel> entity)
    {
        entity.HasKey(e => e.Id).HasName("ai_models_pkey");

        entity.ToTable("ai_models");

        entity.HasIndex(e => e.Code, "ai_models_code_key").IsUnique();

        entity.HasIndex(e => e.CropId, "ix_ai_models_crop");

        entity.HasIndex(e => e.Task, "ix_ai_models_task");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Code).HasColumnName("code");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.Task).HasColumnName("task");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Crop).WithMany(p => p.AiModels)
            .HasForeignKey(d => d.CropId)
            .HasConstraintName("ai_models_crop_id_fkey");
    }
}
