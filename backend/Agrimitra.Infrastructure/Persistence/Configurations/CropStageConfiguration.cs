using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropStageConfiguration : IEntityTypeConfiguration<CropStage>
{
    public void Configure(EntityTypeBuilder<CropStage> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_stages_pkey");

        entity.ToTable("crop_stages");

        entity.HasIndex(e => new { e.CropId, e.Code }, "crop_stages_crop_id_code_key").IsUnique();

        entity.HasIndex(e => new { e.CropId, e.Sequence }, "crop_stages_crop_id_sequence_key").IsUnique();

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
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.NameLocal)
            .HasDefaultValueSql("'{}'::jsonb")
            .HasColumnType("jsonb")
            .HasColumnName("name_local");
        entity.Property(e => e.Sequence).HasColumnName("sequence");
        entity.Property(e => e.TypicalDurationDays).HasColumnName("typical_duration_days");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Crop).WithMany(p => p.CropStages)
            .HasForeignKey(d => d.CropId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_stages_crop_id_fkey");
    }
}
