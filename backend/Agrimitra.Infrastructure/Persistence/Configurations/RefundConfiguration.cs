using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class RefundConfiguration : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> entity)
    {
        entity.HasKey(e => e.Id).HasName("refunds_pkey");

        entity.ToTable("refunds");

        entity.HasIndex(e => e.ApprovedBy, "ix_refunds_approved_by_94ee4d");

        entity.HasIndex(e => e.PaymentId, "ix_refunds_payment");

        entity.HasIndex(e => e.RequestedBy, "ix_refunds_requested_by_6180ed");

        entity.HasIndex(e => new { e.Status, e.CreatedAt }, "ix_refunds_status").HasFilter("(status = ANY (ARRAY['requested'::text, 'approved'::text, 'processing'::text]))");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Amount)
            .HasPrecision(14, 2)
            .HasColumnName("amount");
        entity.Property(e => e.ApprovedAt).HasColumnName("approved_at");
        entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.GatewayRefundId).HasColumnName("gateway_refund_id");
        entity.Property(e => e.PaymentId).HasColumnName("payment_id");
        entity.Property(e => e.ProcessedAt).HasColumnName("processed_at");
        entity.Property(e => e.Reason).HasColumnName("reason");
        entity.Property(e => e.RequestedBy).HasColumnName("requested_by");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'requested'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.RefundApprovedByNavigations)
            .HasForeignKey(d => d.ApprovedBy)
            .HasConstraintName("refunds_approved_by_fkey");

        entity.HasOne(d => d.Payment).WithMany(p => p.Refunds)
            .HasForeignKey(d => d.PaymentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("refunds_payment_id_fkey");

        entity.HasOne(d => d.RequestedByNavigation).WithMany(p => p.RefundRequestedByNavigations)
            .HasForeignKey(d => d.RequestedBy)
            .HasConstraintName("refunds_requested_by_fkey");
    }
}
