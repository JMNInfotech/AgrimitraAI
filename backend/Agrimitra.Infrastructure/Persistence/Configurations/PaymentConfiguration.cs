using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> entity)
    {
        entity.HasKey(e => e.Id).HasName("payments_pkey");

        entity.ToTable("payments");

        entity.HasIndex(e => e.AdBillingId, "ix_payments_ad_billing");

        entity.HasIndex(e => e.ConsultationId, "ix_payments_consultation");

        entity.HasIndex(e => e.LabBookingId, "ix_payments_lab_booking");

        entity.HasIndex(e => e.OrderId, "ix_payments_order");

        entity.HasIndex(e => new { e.PayerUserId, e.CreatedAt }, "ix_payments_payer").IsDescending(false, true);

        entity.HasIndex(e => new { e.Status, e.CreatedAt }, "ix_payments_status").HasFilter("(status = ANY (ARRAY['PENDING'::text, 'PROCESSING'::text]))");

        entity.HasIndex(e => new { e.Gateway, e.GatewayOrderId }, "payments_gateway_gateway_order_id_key").IsUnique();

        entity.HasIndex(e => e.IdempotencyKey, "payments_idempotency_key_key").IsUnique();

        entity.HasIndex(e => e.PaymentNumber, "payments_payment_number_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AdBillingId).HasColumnName("ad_billing_id");
        entity.Property(e => e.Amount)
            .HasPrecision(14, 2)
            .HasColumnName("amount");
        entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValueSql("'INR'::bpchar")
            .IsFixedLength()
            .HasColumnName("currency");
        entity.Property(e => e.FailureReason).HasColumnName("failure_reason");
        entity.Property(e => e.Gateway).HasColumnName("gateway");
        entity.Property(e => e.GatewayOrderId).HasColumnName("gateway_order_id");
        entity.Property(e => e.IdempotencyKey).HasColumnName("idempotency_key");
        entity.Property(e => e.LabBookingId).HasColumnName("lab_booking_id");
        entity.Property(e => e.Method).HasColumnName("method");
        entity.Property(e => e.OrderId).HasColumnName("order_id");
        entity.Property(e => e.PaidAt).HasColumnName("paid_at");
        entity.Property(e => e.PayerUserId).HasColumnName("payer_user_id");
        entity.Property(e => e.PaymentNumber).HasColumnName("payment_number");
        entity.Property(e => e.Purpose).HasColumnName("purpose");
        entity.Property(e => e.RefundedAmount)
            .HasPrecision(14, 2)
            .HasColumnName("refunded_amount");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'PENDING'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.VerifiedAt).HasColumnName("verified_at");

        entity.HasOne(d => d.AdBilling).WithMany(p => p.Payments)
            .HasForeignKey(d => d.AdBillingId)
            .HasConstraintName("fk_payments_ad_billing");

        entity.HasOne(d => d.Consultation).WithMany(p => p.Payments)
            .HasForeignKey(d => d.ConsultationId)
            .HasConstraintName("payments_consultation_id_fkey");

        entity.HasOne(d => d.LabBooking).WithMany(p => p.Payments)
            .HasForeignKey(d => d.LabBookingId)
            .HasConstraintName("payments_lab_booking_id_fkey");

        entity.HasOne(d => d.Order).WithMany(p => p.Payments)
            .HasForeignKey(d => d.OrderId)
            .HasConstraintName("payments_order_id_fkey");

        entity.HasOne(d => d.PayerUser).WithMany(p => p.Payments)
            .HasForeignKey(d => d.PayerUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("payments_payer_user_id_fkey");
    }
}
