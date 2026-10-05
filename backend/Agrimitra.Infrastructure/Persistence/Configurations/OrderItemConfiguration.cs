using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> entity)
    {
        entity.HasKey(e => e.Id).HasName("order_items_pkey");

        entity.ToTable("order_items");

        entity.HasIndex(e => e.NurseryBatchId, "ix_order_items_nursery_batch");

        entity.HasIndex(e => e.OrderId, "ix_order_items_order");

        entity.HasIndex(e => e.ProductId, "ix_order_items_product");

        entity.HasIndex(e => e.ProductBatchId, "ix_order_items_product_batch");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.LineTotal)
            .HasPrecision(14, 2)
            .HasComputedColumnSql("round((((quantity)::numeric * unit_price) * ((1)::numeric + (tax_percent / (100)::numeric))), 2)", true)
            .HasColumnName("line_total");
        entity.Property(e => e.NurseryBatchId).HasColumnName("nursery_batch_id");
        entity.Property(e => e.OrderId).HasColumnName("order_id");
        entity.Property(e => e.ProductBatchId).HasColumnName("product_batch_id");
        entity.Property(e => e.ProductId).HasColumnName("product_id");
        entity.Property(e => e.Quantity).HasColumnName("quantity");
        entity.Property(e => e.TaxPercent)
            .HasPrecision(5, 2)
            .HasColumnName("tax_percent");
        entity.Property(e => e.TitleSnapshot).HasColumnName("title_snapshot");
        entity.Property(e => e.UnitPrice)
            .HasPrecision(12, 2)
            .HasColumnName("unit_price");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.NurseryBatch).WithMany(p => p.OrderItems)
            .HasForeignKey(d => d.NurseryBatchId)
            .HasConstraintName("order_items_nursery_batch_id_fkey");

        entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
            .HasForeignKey(d => d.OrderId)
            .HasConstraintName("order_items_order_id_fkey");

        entity.HasOne(d => d.ProductBatch).WithMany(p => p.OrderItems)
            .HasForeignKey(d => d.ProductBatchId)
            .HasConstraintName("order_items_product_batch_id_fkey");

        entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("order_items_product_id_fkey");
    }
}
