using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class NurseryProductConfiguration : IEntityTypeConfiguration<NurseryProduct>
{
    public void Configure(EntityTypeBuilder<NurseryProduct> entity)
    {
        entity.HasKey(e => e.Id).HasName("nursery_products_pkey");

        entity.ToTable("nursery_products");

        entity.HasIndex(e => e.CategoryId, "ix_nursery_products_category");

        entity.HasIndex(e => e.CropId, "ix_nursery_products_crop");

        entity.HasIndex(e => e.Name, "ix_nursery_products_name_trgm")
            .HasMethod("gin")
            .HasOperators(new[] { "gin_trgm_ops" });

        entity.HasIndex(e => e.NurseryId, "ix_nursery_products_nursery").HasFilter("(NOT is_deleted)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CategoryId).HasColumnName("category_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.IsAvailable)
            .HasDefaultValue(true)
            .HasColumnName("is_available");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.NurseryId).HasColumnName("nursery_id");
        entity.Property(e => e.Unit)
            .HasDefaultValueSql("'piece'::text")
            .HasColumnName("unit");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Category).WithMany(p => p.NurseryProducts)
            .HasForeignKey(d => d.CategoryId)
            .HasConstraintName("nursery_products_category_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.NurseryProducts)
            .HasForeignKey(d => d.CropId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("nursery_products_crop_id_fkey");

        entity.HasOne(d => d.Nursery).WithMany(p => p.NurseryProducts)
            .HasForeignKey(d => d.NurseryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("nursery_products_nursery_id_fkey");
    }
}
