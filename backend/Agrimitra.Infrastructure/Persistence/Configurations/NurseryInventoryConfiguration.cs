using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class NurseryInventoryConfiguration : IEntityTypeConfiguration<NurseryInventory>
{
    public void Configure(EntityTypeBuilder<NurseryInventory> entity)
    {
        entity.HasKey(e => e.BatchId).HasName("nursery_inventory_pkey");

        entity.ToTable("nursery_inventory");

        entity.Property(e => e.BatchId)
            .ValueGeneratedNever()
            .HasColumnName("batch_id");
        entity.Property(e => e.LowStockThreshold).HasColumnName("low_stock_threshold");
        entity.Property(e => e.QuantityDamaged).HasColumnName("quantity_damaged");
        entity.Property(e => e.QuantityOnHand).HasColumnName("quantity_on_hand");
        entity.Property(e => e.QuantityReserved).HasColumnName("quantity_reserved");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Batch).WithOne(p => p.NurseryInventory)
            .HasForeignKey<NurseryInventory>(d => d.BatchId)
            .HasConstraintName("nursery_inventory_batch_id_fkey");
    }
}
