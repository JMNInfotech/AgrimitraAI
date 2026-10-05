using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
        entity.HasKey(e => e.Id).HasName("products_pkey");

        entity.ToTable("products");

        entity.HasIndex(e => new { e.CategoryId, e.Price }, "ix_products_category_price").HasFilter("((status = 'active'::text) AND (NOT is_deleted))");

        entity.HasIndex(e => e.ConsultantId, "ix_products_consultant");

        entity.HasIndex(e => e.CropId, "ix_products_crop");

        entity.HasIndex(e => e.LaboratoryId, "ix_products_laboratory");

        entity.HasIndex(e => e.NurseryId, "ix_products_nursery");

        entity.HasIndex(e => e.SearchVector, "ix_products_search").HasMethod("gin");

        entity.HasIndex(e => new { e.SellerType, e.Status }, "ix_products_seller_type");

        entity.HasIndex(e => e.ShopId, "ix_products_shop");

        entity.HasIndex(e => e.Title, "ix_products_title_trgm")
            .HasMethod("gin")
            .HasOperators(new[] { "gin_trgm_ops" });

        entity.HasIndex(e => e.ConsultationServiceId, "ux_products_consult_service")
            .IsUnique()
            .HasFilter("((consultation_service_id IS NOT NULL) AND (NOT is_deleted))");

        entity.HasIndex(e => e.LaboratoryServiceId, "ux_products_lab_service")
            .IsUnique()
            .HasFilter("((laboratory_service_id IS NOT NULL) AND (NOT is_deleted))");

        entity.HasIndex(e => e.NurseryBatchId, "ux_products_nursery_batch")
            .IsUnique()
            .HasFilter("((nursery_batch_id IS NOT NULL) AND (NOT is_deleted))");

        entity.HasIndex(e => e.ShopProductId, "ux_products_shop_product")
            .IsUnique()
            .HasFilter("((shop_product_id IS NOT NULL) AND (NOT is_deleted))");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CategoryId).HasColumnName("category_id");
        entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
        entity.Property(e => e.ConsultationServiceId).HasColumnName("consultation_service_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValueSql("'INR'::bpchar")
            .IsFixedLength()
            .HasColumnName("currency");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.IsAvailable)
            .HasDefaultValue(true)
            .HasColumnName("is_available");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.IsVerifiedSeller).HasColumnName("is_verified_seller");
        entity.Property(e => e.Kind).HasColumnName("kind");
        entity.Property(e => e.LaboratoryId).HasColumnName("laboratory_id");
        entity.Property(e => e.LaboratoryServiceId).HasColumnName("laboratory_service_id");
        entity.Property(e => e.NurseryBatchId).HasColumnName("nursery_batch_id");
        entity.Property(e => e.NurseryId).HasColumnName("nursery_id");
        entity.Property(e => e.Price)
            .HasPrecision(12, 2)
            .HasColumnName("price");
        entity.Property(e => e.RatingAverage)
            .HasPrecision(3, 2)
            .HasColumnName("rating_average");
        entity.Property(e => e.RatingCount).HasColumnName("rating_count");
        entity.Property(e => e.SearchVector)
            .HasComputedColumnSql("to_tsvector('simple'::regconfig, ((COALESCE(title, ''::text) || ' '::text) || COALESCE(description, ''::text)))", true)
            .HasColumnName("search_vector");
        entity.Property(e => e.SellerType).HasColumnName("seller_type");
        entity.Property(e => e.ShopId).HasColumnName("shop_id");
        entity.Property(e => e.ShopProductId).HasColumnName("shop_product_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'active'::text")
            .HasColumnName("status");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.Unit).HasColumnName("unit");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Category).WithMany(p => p.Products)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("products_category_id_fkey");

        entity.HasOne(d => d.Consultant).WithMany(p => p.Products)
            .HasForeignKey(d => d.ConsultantId)
            .HasConstraintName("products_consultant_id_fkey");

        entity.HasOne(d => d.ConsultationService).WithOne(p => p.Product)
            .HasForeignKey<Product>(d => d.ConsultationServiceId)
            .HasConstraintName("products_consultation_service_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.Products)
            .HasForeignKey(d => d.CropId)
            .HasConstraintName("products_crop_id_fkey");

        entity.HasOne(d => d.Laboratory).WithMany(p => p.Products)
            .HasForeignKey(d => d.LaboratoryId)
            .HasConstraintName("products_laboratory_id_fkey");

        entity.HasOne(d => d.LaboratoryService).WithOne(p => p.Product)
            .HasForeignKey<Product>(d => d.LaboratoryServiceId)
            .HasConstraintName("products_laboratory_service_id_fkey");

        entity.HasOne(d => d.NurseryBatch).WithOne(p => p.Product)
            .HasForeignKey<Product>(d => d.NurseryBatchId)
            .HasConstraintName("products_nursery_batch_id_fkey");

        entity.HasOne(d => d.Nursery).WithMany(p => p.Products)
            .HasForeignKey(d => d.NurseryId)
            .HasConstraintName("products_nursery_id_fkey");

        entity.HasOne(d => d.Shop).WithMany(p => p.Products)
            .HasForeignKey(d => d.ShopId)
            .HasConstraintName("products_shop_id_fkey");

        entity.HasOne(d => d.ShopProduct).WithOne(p => p.Product)
            .HasForeignKey<Product>(d => d.ShopProductId)
            .HasConstraintName("products_shop_product_id_fkey");
    }
}
