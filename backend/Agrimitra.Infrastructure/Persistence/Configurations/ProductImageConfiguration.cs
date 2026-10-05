using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> entity)
    {
        entity.HasKey(e => e.Id).HasName("product_images_pkey");

        entity.ToTable("product_images");

        entity.HasIndex(e => e.FileObjectId, "ix_product_images_file_object_id_1177ba");

        entity.HasIndex(e => new { e.ProductId, e.SortOrder }, "ix_product_images_product");

        entity.HasIndex(e => e.ProductId, "ux_product_images_primary")
            .IsUnique()
            .HasFilter("is_primary");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AltText).HasColumnName("alt_text");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.IsPrimary).HasColumnName("is_primary");
        entity.Property(e => e.ProductId).HasColumnName("product_id");
        entity.Property(e => e.SortOrder).HasColumnName("sort_order");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.FileObject).WithMany(p => p.ProductImages)
            .HasForeignKey(d => d.FileObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("product_images_file_object_id_fkey");

        entity.HasOne(d => d.Product).WithOne(p => p.ProductImage)
            .HasForeignKey<ProductImage>(d => d.ProductId)
            .HasConstraintName("product_images_product_id_fkey");
    }
}
