using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> entity)
    {
        entity.HasKey(e => e.Id).HasName("shipments_pkey");

        entity.ToTable("shipments");

        entity.HasIndex(e => e.OrderId, "ix_shipments_order");

        entity.HasIndex(e => e.TrackingNumber, "ix_shipments_tracking").HasFilter("(tracking_number IS NOT NULL)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Carrier).HasColumnName("carrier");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.EstimatedDeliveryOn).HasColumnName("estimated_delivery_on");
        entity.Property(e => e.OrderId).HasColumnName("order_id");
        entity.Property(e => e.ShippedAt).HasColumnName("shipped_at");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'preparing'::text")
            .HasColumnName("status");
        entity.Property(e => e.TrackingNumber).HasColumnName("tracking_number");
        entity.Property(e => e.TrackingUrl).HasColumnName("tracking_url");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Order).WithMany(p => p.Shipments)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("shipments_order_id_fkey");
    }
}
