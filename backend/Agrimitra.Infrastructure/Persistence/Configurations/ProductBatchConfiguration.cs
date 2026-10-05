using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ProductBatchConfiguration : IEntityTypeConfiguration<ProductBatch>
{
    public void Configure(EntityTypeBuilder<ProductBatch> entity)
    {
        entity.HasKey(e => e.Id).HasName("product_batches_pkey");

        entity.ToTable("product_batches");

        entity.HasIndex(e => e.ExpiryDate, "ix_product_batches_expiry").HasFilter("(status = 'active'::text)");

        entity.HasIndex(e => new { e.ShopProductId, e.BatchNumber }, "product_batches_shop_product_id_batch_number_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.BatchNumber).HasColumnName("batch_number");
        entity.Property(e => e.CostPrice)
            .HasPrecision(12, 2)
            .HasColumnName("cost_price");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.ManufacturedOn).HasColumnName("manufactured_on");
        entity.Property(e => e.QuantityAvailable).HasColumnName("quantity_available");
        entity.Property(e => e.QuantityReceived).HasColumnName("quantity_received");
        entity.Property(e => e.ShopProductId).HasColumnName("shop_product_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'active'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.ShopProduct).WithMany(p => p.ProductBatches)
            .HasForeignKey(d => d.ShopProductId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("product_batches_shop_product_id_fkey");
    }
}
