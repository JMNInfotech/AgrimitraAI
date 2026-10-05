using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropDiseaseConfiguration : IEntityTypeConfiguration<CropDisease>
{
    public void Configure(EntityTypeBuilder<CropDisease> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_diseases_pkey");

        entity.ToTable("crop_diseases");

        entity.HasIndex(e => e.Code, "crop_diseases_code_key").IsUnique();

        entity.HasIndex(e => e.CropId, "ix_crop_diseases_crop");

        entity.HasIndex(e => e.Kind, "ix_crop_diseases_kind");

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
        entity.Property(e => e.Kind).HasColumnName("kind");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.NameLocal)
            .HasDefaultValueSql("'{}'::jsonb")
            .HasColumnType("jsonb")
            .HasColumnName("name_local");
        entity.Property(e => e.Prevention).HasColumnName("prevention");
        entity.Property(e => e.ScientificName).HasColumnName("scientific_name");
        entity.Property(e => e.Symptoms).HasColumnName("symptoms");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Crop).WithMany(p => p.CropDiseases)
            .HasForeignKey(d => d.CropId)
            .HasConstraintName("crop_diseases_crop_id_fkey");
    }
}
