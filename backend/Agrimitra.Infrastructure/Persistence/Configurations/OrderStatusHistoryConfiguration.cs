using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<OrderStatusHistory> entity)
    {
        entity.HasKey(e => e.Id).HasName("order_status_history_pkey");

        entity.ToTable("order_status_history");

        entity.HasIndex(e => e.ChangedBy, "ix_order_status_history_changed_by_3a3034");

        entity.HasIndex(e => new { e.OrderId, e.CreatedAt }, "ix_order_status_history_order");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.FromStatus).HasColumnName("from_status");
        entity.Property(e => e.OrderId).HasColumnName("order_id");
        entity.Property(e => e.Reason).HasColumnName("reason");
        entity.Property(e => e.ToStatus).HasColumnName("to_status");

        entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.OrderStatusHistories)
            .HasForeignKey(d => d.ChangedBy)
            .HasConstraintName("order_status_history_changed_by_fkey");

        entity.HasOne(d => d.Order).WithMany(p => p.OrderStatusHistories)
            .HasForeignKey(d => d.OrderId)
            .HasConstraintName("order_status_history_order_id_fkey");
    }
}
