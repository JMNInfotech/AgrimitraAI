using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> entity)
    {
        entity.HasKey(e => e.Id).HasName("cart_items_pkey");

        entity.ToTable("cart_items");

        entity.HasIndex(e => new { e.CartId, e.ProductId }, "cart_items_cart_id_product_id_key").IsUnique();

        entity.HasIndex(e => e.ProductId, "ix_cart_items_product");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CartId).HasColumnName("cart_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.ProductId).HasColumnName("product_id");
        entity.Property(e => e.Quantity).HasColumnName("quantity");
        entity.Property(e => e.UnitPriceSnapshot)
            .HasPrecision(12, 2)
            .HasColumnName("unit_price_snapshot");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
            .HasForeignKey(d => d.CartId)
            .HasConstraintName("cart_items_cart_id_fkey");

        entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("cart_items_product_id_fkey");
    }
}
