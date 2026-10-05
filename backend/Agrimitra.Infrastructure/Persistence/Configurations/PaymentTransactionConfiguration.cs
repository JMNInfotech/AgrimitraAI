using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> entity)
    {
        entity.HasKey(e => e.Id).HasName("payment_transactions_pkey");

        entity.ToTable("payment_transactions");

        entity.HasIndex(e => new { e.PaymentId, e.CreatedAt }, "ix_payment_transactions_payment");

        entity.HasIndex(e => new { e.PaymentId, e.Kind, e.GatewayTransactionId }, "ux_payment_transactions_gateway")
            .IsUnique()
            .HasFilter("(gateway_transaction_id IS NOT NULL)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Amount)
            .HasPrecision(14, 2)
            .HasColumnName("amount");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.GatewayTransactionId).HasColumnName("gateway_transaction_id");
        entity.Property(e => e.Kind).HasColumnName("kind");
        entity.Property(e => e.PaymentId).HasColumnName("payment_id");
        entity.Property(e => e.RawResponse)
            .HasColumnType("jsonb")
            .HasColumnName("raw_response");
        entity.Property(e => e.SignatureVerified).HasColumnName("signature_verified");
        entity.Property(e => e.Status).HasColumnName("status");

        entity.HasOne(d => d.Payment).WithMany(p => p.PaymentTransactions)
            .HasForeignKey(d => d.PaymentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("payment_transactions_payment_id_fkey");
    }
}
