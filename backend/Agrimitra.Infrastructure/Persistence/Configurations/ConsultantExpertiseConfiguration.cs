using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsultantExpertiseConfiguration : IEntityTypeConfiguration<ConsultantExpertise>
{
    public void Configure(EntityTypeBuilder<ConsultantExpertise> entity)
    {
        entity.HasKey(e => e.Id).HasName("consultant_expertise_pkey");

        entity.ToTable("consultant_expertise");

        entity.HasIndex(e => e.ConsultantId, "ix_consultant_expertise_consultant");

        entity.HasIndex(e => e.CropId, "ix_consultant_expertise_crop");

        entity.HasIndex(e => e.CropDiseaseId, "ix_consultant_expertise_disease");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropDiseaseId).HasColumnName("crop_disease_id");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.Topic).HasColumnName("topic");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Consultant).WithMany(p => p.ConsultantExpertises)
            .HasForeignKey(d => d.ConsultantId)
            .HasConstraintName("consultant_expertise_consultant_id_fkey");

        entity.HasOne(d => d.CropDisease).WithMany(p => p.ConsultantExpertises)
            .HasForeignKey(d => d.CropDiseaseId)
            .HasConstraintName("consultant_expertise_crop_disease_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.ConsultantExpertises)
            .HasForeignKey(d => d.CropId)
            .HasConstraintName("consultant_expertise_crop_id_fkey");
    }
}
