using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ShopProductConfiguration : IEntityTypeConfiguration<ShopProduct>
{
    public void Configure(EntityTypeBuilder<ShopProduct> entity)
    {
        entity.HasKey(e => e.Id).HasName("shop_products_pkey");

        entity.ToTable("shop_products");

        entity.HasIndex(e => e.CategoryId, "ix_shop_products_category");

        entity.HasIndex(e => e.ImageFileId, "ix_shop_products_image_file_id_6197b2");

        entity.HasIndex(e => e.ManufacturerId, "ix_shop_products_manufacturer");

        entity.HasIndex(e => e.SearchVector, "ix_shop_products_search").HasMethod("gin");

        entity.HasIndex(e => new { e.ShopId, e.IsAvailable }, "ix_shop_products_shop").HasFilter("(NOT is_deleted)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActiveIngredient).HasColumnName("active_ingredient");
        entity.Property(e => e.Brand).HasColumnName("brand");
        entity.Property(e => e.CategoryId).HasColumnName("category_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValueSql("'INR'::bpchar")
            .IsFixedLength()
            .HasColumnName("currency");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.Formulation).HasColumnName("formulation");
        entity.Property(e => e.ImageFileId).HasColumnName("image_file_id");
        entity.Property(e => e.IsAvailable)
            .HasDefaultValue(true)
            .HasColumnName("is_available");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.IsRestricted).HasColumnName("is_restricted");
        entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
        entity.Property(e => e.Mrp)
            .HasPrecision(12, 2)
            .HasColumnName("mrp");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.PackSize)
            .HasPrecision(12, 3)
            .HasColumnName("pack_size");
        entity.Property(e => e.PackUnit).HasColumnName("pack_unit");
        entity.Property(e => e.Price)
            .HasPrecision(12, 2)
            .HasColumnName("price");
        entity.Property(e => e.RegistrationNumber).HasColumnName("registration_number");
        entity.Property(e => e.SearchVector)
            .HasComputedColumnSql("to_tsvector('simple'::regconfig, ((((COALESCE(name, ''::text) || ' '::text) || COALESCE(brand, ''::text)) || ' '::text) || COALESCE(active_ingredient, ''::text)))", true)
            .HasColumnName("search_vector");
        entity.Property(e => e.ShopId).HasColumnName("shop_id");
        entity.Property(e => e.TaxPercent)
            .HasPrecision(5, 2)
            .HasColumnName("tax_percent");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Category).WithMany(p => p.ShopProducts)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("shop_products_category_id_fkey");

        entity.HasOne(d => d.ImageFile).WithMany(p => p.ShopProducts)
            .HasForeignKey(d => d.ImageFileId)
            .HasConstraintName("shop_products_image_file_id_fkey");

        entity.HasOne(d => d.Manufacturer).WithMany(p => p.ShopProducts)
            .HasForeignKey(d => d.ManufacturerId)
            .HasConstraintName("shop_products_manufacturer_id_fkey");

        entity.HasOne(d => d.Shop).WithMany(p => p.ShopProducts)
            .HasForeignKey(d => d.ShopId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("shop_products_shop_id_fkey");
    }
}
