using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ProductInventoryConfiguration : IEntityTypeConfiguration<ProductInventory>
{
    public void Configure(EntityTypeBuilder<ProductInventory> entity)
    {
        entity.HasKey(e => e.ShopProductId).HasName("product_inventory_pkey");

        entity.ToTable("product_inventory");

        entity.Property(e => e.ShopProductId)
            .ValueGeneratedNever()
            .HasColumnName("shop_product_id");
        entity.Property(e => e.LowStockThreshold).HasColumnName("low_stock_threshold");
        entity.Property(e => e.QuantityOnHand).HasColumnName("quantity_on_hand");
        entity.Property(e => e.QuantityReserved).HasColumnName("quantity_reserved");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.ShopProduct).WithOne(p => p.ProductInventory)
            .HasForeignKey<ProductInventory>(d => d.ShopProductId)
            .HasConstraintName("product_inventory_shop_product_id_fkey");
    }
}
