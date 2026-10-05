using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class NurseryProductVarietyConfiguration : IEntityTypeConfiguration<NurseryProductVariety>
{
    public void Configure(EntityTypeBuilder<NurseryProductVariety> entity)
    {
        entity.HasKey(e => e.Id).HasName("nursery_product_varieties_pkey");

        entity.ToTable("nursery_product_varieties");

        entity.HasIndex(e => e.NurseryProductId, "ix_nursery_product_varieties_product");

        entity.HasIndex(e => e.CropVarietyId, "ix_nursery_product_varieties_variety");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropVarietyId).HasColumnName("crop_variety_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.NurseryProductId).HasColumnName("nursery_product_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.CropVariety).WithMany(p => p.NurseryProductVarieties)
            .HasForeignKey(d => d.CropVarietyId)
            .HasConstraintName("nursery_product_varieties_crop_variety_id_fkey");

        entity.HasOne(d => d.NurseryProduct).WithMany(p => p.NurseryProductVarieties)
            .HasForeignKey(d => d.NurseryProductId)
            .HasConstraintName("nursery_product_varieties_nursery_product_id_fkey");
    }
}
