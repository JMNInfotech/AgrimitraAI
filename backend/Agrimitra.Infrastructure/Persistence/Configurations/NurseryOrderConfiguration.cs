using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class NurseryOrderConfiguration : IEntityTypeConfiguration<NurseryOrder>
{
    public void Configure(EntityTypeBuilder<NurseryOrder> entity)
    {
        entity.HasKey(e => e.Id).HasName("nursery_orders_pkey");

        entity.ToTable("nursery_orders");

        entity.HasIndex(e => new { e.NurseryId, e.Status }, "ix_nursery_orders_nursery");

        entity.HasIndex(e => e.OrderId, "nursery_orders_order_id_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.NurseryId).HasColumnName("nursery_id");
        entity.Property(e => e.OrderId).HasColumnName("order_id");
        entity.Property(e => e.PickupSlotEnd).HasColumnName("pickup_slot_end");
        entity.Property(e => e.PickupSlotStart).HasColumnName("pickup_slot_start");
        entity.Property(e => e.ReadyOn).HasColumnName("ready_on");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'received'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Nursery).WithMany(p => p.NurseryOrders)
            .HasForeignKey(d => d.NurseryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("nursery_orders_nursery_id_fkey");

        entity.HasOne(d => d.Order).WithOne(p => p.NurseryOrder)
            .HasForeignKey<NurseryOrder>(d => d.OrderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("nursery_orders_order_id_fkey");
    }
}
