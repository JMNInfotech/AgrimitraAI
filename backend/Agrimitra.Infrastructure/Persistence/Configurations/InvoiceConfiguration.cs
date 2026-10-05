using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> entity)
    {
        entity.HasKey(e => e.Id).HasName("invoices_pkey");

        entity.ToTable("invoices");

        entity.HasIndex(e => e.InvoiceNumber, "invoices_invoice_number_key").IsUnique();

        entity.HasIndex(e => e.AdBillingId, "ix_invoices_ad_billing");

        entity.HasIndex(e => e.ConsultationId, "ix_invoices_consultation");

        entity.HasIndex(e => e.LabBookingId, "ix_invoices_lab_booking");

        entity.HasIndex(e => e.OrderId, "ix_invoices_order");

        entity.HasIndex(e => e.PaymentId, "ix_invoices_payment");

        entity.HasIndex(e => e.PdfFileObjectId, "ix_invoices_pdf_file_object_id_50bce4");

        entity.HasIndex(e => new { e.BillToUserId, e.IssuedAt }, "ix_invoices_user").IsDescending(false, true);

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AdBillingId).HasColumnName("ad_billing_id");
        entity.Property(e => e.BillToUserId).HasColumnName("bill_to_user_id");
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
        entity.Property(e => e.InvoiceNumber).HasColumnName("invoice_number");
        entity.Property(e => e.InvoiceType).HasColumnName("invoice_type");
        entity.Property(e => e.IssuedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("issued_at");
        entity.Property(e => e.IssuerGstin).HasColumnName("issuer_gstin");
        entity.Property(e => e.IssuerName).HasColumnName("issuer_name");
        entity.Property(e => e.LabBookingId).HasColumnName("lab_booking_id");
        entity.Property(e => e.OrderId).HasColumnName("order_id");
        entity.Property(e => e.PaymentId).HasColumnName("payment_id");
        entity.Property(e => e.PdfFileObjectId).HasColumnName("pdf_file_object_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'issued'::text")
            .HasColumnName("status");
        entity.Property(e => e.Subtotal)
            .HasPrecision(14, 2)
            .HasColumnName("subtotal");
        entity.Property(e => e.TaxAmount)
            .HasPrecision(14, 2)
            .HasColumnName("tax_amount");
        entity.Property(e => e.TotalAmount)
            .HasPrecision(14, 2)
            .HasComputedColumnSql("(subtotal + tax_amount)", true)
            .HasColumnName("total_amount");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.AdBilling).WithMany(p => p.Invoices)
            .HasForeignKey(d => d.AdBillingId)
            .HasConstraintName("fk_invoices_ad_billing");

        entity.HasOne(d => d.BillToUser).WithMany(p => p.Invoices)
            .HasForeignKey(d => d.BillToUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("invoices_bill_to_user_id_fkey");

        entity.HasOne(d => d.Consultation).WithMany(p => p.Invoices)
            .HasForeignKey(d => d.ConsultationId)
            .HasConstraintName("invoices_consultation_id_fkey");

        entity.HasOne(d => d.LabBooking).WithMany(p => p.Invoices)
            .HasForeignKey(d => d.LabBookingId)
            .HasConstraintName("invoices_lab_booking_id_fkey");

        entity.HasOne(d => d.Order).WithMany(p => p.Invoices)
            .HasForeignKey(d => d.OrderId)
            .HasConstraintName("invoices_order_id_fkey");

        entity.HasOne(d => d.Payment).WithMany(p => p.Invoices)
            .HasForeignKey(d => d.PaymentId)
            .HasConstraintName("invoices_payment_id_fkey");

        entity.HasOne(d => d.PdfFileObject).WithMany(p => p.Invoices)
            .HasForeignKey(d => d.PdfFileObjectId)
            .HasConstraintName("invoices_pdf_file_object_id_fkey");
    }
}
